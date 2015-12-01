using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;


namespace SpaceshipDrawer.Render
{
    class RenderDirectX
    {
        public static TestDraw()
        {
            // В этот список мы будем добавлять все созданные по ходу дела ресурсы
            // дабы не забыть их освободить
            var unmanagedResources = new List<IDisposable>();

            // Создадим окно. В классе RenderForm, описанном в SharpDX, уже реализован MessagePump,
            // так что это избавит нас от лишней рутины и позволит сконцентрироваться на главном.
            var form = new RenderForm("Example");

            // Опишем параметры SwapChain - набора буферов для отображения результирующей картинки
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(
                                   form.ClientSize.Width,
                                   form.ClientSize.Height,
                                   new Rational(0, 1),
                                   Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Инициализируем устройство и SwapChain
            // И получим контекст устройства
            Device device;
            SwapChain swapChain;

            Device.CreateWithSwapChain(
                DriverType.Hardware,
                DeviceCreationFlags.None,
                new[] { FeatureLevel.Level_10_0 },
                desc,
                out device,
                out swapChain);

            var context = device.ImmediateContext;

            unmanagedResources.Add(device);
            unmanagedResources.Add(swapChain);
            unmanagedResources.Add(context);

            // Игнорируем все события окна
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);
            unmanagedResources.Add(factory);

            // Получаем буфер для рендеринга итоговой картинки
            var backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0);
            unmanagedResources.Add(backBuffer);

            // Устанавливаем этот буфер как наш выход для рендера
            var renderView = new RenderTargetView(device, backBuffer);
            unmanagedResources.Add(renderView);

            // Скомпилируем эффект из файла в байткод
            ShaderBytecode bytecode = ShaderBytecode.CompileFromFile("shader.fx", "fx_5_0");
            unmanagedResources.Add(bytecode);

            // Загрузим скомпилированный эффект в видеокарту
            var renderEffect = new Effect(device, bytecode);
            unmanagedResources.Add(renderEffect);

            // Выберем технику рендера (у нас она одна)
            // Мы можем описать в файле эффекта несколько техник,
            // например для разного уровня аппаратной поддержки
            var renderTechnique = renderEffect.GetTechniqueByName("SimpleRedRender");
            unmanagedResources.Add(renderTechnique);

            // Выберем входную сигнатуру данных первого прохода
            var renderPassSignature = renderTechnique.GetPassByIndex(0).Description.Signature;
            unmanagedResources.Add(renderPassSignature);

            // Определяем формат вершинного буфера для входной сигнатуры первого прохода
            var inputLayout = new InputLayout(
                device,
                renderPassSignature,
                new[]
                    {
                        // Собственно у нас весь буфер состоит из элементов одного типа - 
                        // позиции вершины.
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                    });
            unmanagedResources.Add(inputLayout);

            // Создадим вершинный буфер из трех вершин для одного треугольника
            // Я использовал числа меньшие, чем в статье, чтобы влезло на экран
            var vertices = Buffer.Create(device, BindFlags.VertexBuffer, new[]
                                  {
                                      new Vector3(0.0f, 0.5f, 0f),
                                      new Vector3(-0.5f, -0.5f, 0f),
                                      new Vector3(0.5f, -0.5f, 0f)
                                  });
            unmanagedResources.Add(vertices);

            // Создадим индексный буфер, описывающий один треугольник
            var indices = Buffer.Create(device, BindFlags.IndexBuffer, new uint[] { 0, 2, 1 });
            unmanagedResources.Add(indices);

            // Установим формат вершинного буфера
            context.InputAssembler.InputLayout = inputLayout;

            // Установим тип примитивов в индексом буфере - список треугольников
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            // Установим текущим вершинный буфер
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 12, 0));

            // Установим текущим индексный буфер с описанием треугольника, и укажем тип данных индекса (32-бит unsigned int)
            context.InputAssembler.SetIndexBuffer(indices, Format.R32_UInt, 0);

            // Устаналиваем вьюпорт (координаты и размер области вывода растеризатора)
            context.Rasterizer.SetViewports(new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0.0f, 1.0f));

            // Установим RenderTargetView в который рендерить картинку
            context.OutputMerger.SetTargets(renderView);

            // Цикл, пока не закроем окно
            RenderLoop.Run(form, () =>
            {
                // Очистим RenderTargetView темно-синим цветом
                // Почему не черный - когда черный цвет, не всегда понятно, действительно ли на экран ничего
                // не выводится, или у нас некорректно работает пиксельный шейдер и черные объекты сливаются с фоном.
                // Если же фон не черный - мы всегда увидим, если что-то вообще рендерится.
                context.ClearRenderTargetView(renderView, Colors.DarkSlateBlue);

                // Вызовем команду отрисовки индексированного примитива для каждого прохода,
                // описанного в эффекте (в данном случае он у нас только один)
                for (int i = 0; i < renderTechnique.Description.PassCount; i++)
                {
                    // Установим текущим описание прохода под номером i нашего эффекта
                    renderTechnique.GetPassByIndex(i).Apply(context);

                    // Отрендерим индексированный примитив, используя установленные ранее буферы и проход
                    // 3 - это количество индексов из буфера, то есть в нашем случае все.
                    context.DrawIndexed(3, 0, 0);
                }

                // Отобразим результат (переключим back и front буферы)
                swapChain.Present(0, PresentFlags.None);
            });

            // Освободим ресурсы
            foreach (var unmanagedResource in unmanagedResources)
            {
                unmanagedResource.Dispose();
            }

        }

    }
}
