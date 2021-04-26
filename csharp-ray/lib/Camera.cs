using System;
using System.Numerics;

namespace csharp_ray
{
    public class Camera
    {
        Vector3 origin;
        Vector3 horizontal, vertical, lower_left_corner;

        public Camera()
        {
            float aspect_ratio = 16.0f/9.0f;
            float viewport_height = 2.0f;
            float viewport_width  = aspect_ratio * viewport_height; 
            float focal_length = 1.0f;

            origin = new Vector3(0.0f, 0.0f, 0.0f);
            horizontal = new Vector3(viewport_width, 0.0f, 0.0f);
            vertical = new Vector3(0.0f, viewport_height, 0.0f);
            lower_left_corner = origin - horizontal/2.0f - vertical/2.0f - new Vector3(0.0f, 0.0f, focal_length);
        }

        public Ray GetRay(float u, float v) {
            return new Ray(origin, lower_left_corner + u*horizontal + v*vertical - origin);
        }
    }
}