using System;

namespace csharp_ray
{
    class Program
    {
        static void Main(string[] args)
        {       
            // int image_width = 1920;
            // int image_height = 1080;

            var tracer = new RayTracer(400);

            tracer.render();
        }
    }
}
