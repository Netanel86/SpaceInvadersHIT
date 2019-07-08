namespace Ex03.Infrastracture.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }
        
        Vector2 Velocity { get; }
    }

    public interface ICollidablePerPixel : ICollidable2D
    {
        Color[] PixelMap { get; set; }

        /// <summary>
        /// a nullable <typeparamref name="Vector2D"/> representing the point(x,y) of collision
        /// in screen boundries.
        /// </summary>
        Vector2? PointOfImpact { get; set; }
    }
}
