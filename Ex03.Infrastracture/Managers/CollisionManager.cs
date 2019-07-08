namespace Ex03.Infrastracture.Managers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ServiceInterfaces;

    public class CollisionManager : GameService, ICollisionManager
    {
        private readonly List<ICollidable> r_Collidables;
        
        public CollisionManager(Game i_Game)
            : base(i_Game)
        {
            r_Collidables = new List<ICollidable>();
        }

        public void AddCollidable(ICollidable i_CollidableComponent)
        {
            if (!r_Collidables.Contains(i_CollidableComponent))
            {
                this.r_Collidables.Add(i_CollidableComponent);
                i_CollidableComponent.SizeChanged += collidable_PropertyChanged;
                i_CollidableComponent.PositionChanged += collidable_PropertyChanged;
                i_CollidableComponent.VisibleChanged += collidable_PropertyChanged;
                i_CollidableComponent.Disposed += collidable_Disposed;
            }
            else
            {
                throw new ObjectDuplicateException(i_CollidableComponent, r_Collidables);
            }
        }
        
        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionManager), this);
        }
        
        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collided = sender as ICollidable;
            if (collided != null && r_Collidables.Contains(collided))
            {
                collided.Disposed -= collidable_Disposed;
                collided.SizeChanged -= collidable_PropertyChanged;
                collided.PositionChanged -= collidable_PropertyChanged;
                collided.VisibleChanged -= collidable_PropertyChanged;
                this.r_Collidables.Remove(collided);
            }
        }

        private void collidable_PropertyChanged(object sender, EventArgs e)
        {
            ICollidable senderCollidable = sender as ICollidable;
            if (senderCollidable != null)
            {
                checkCollision(senderCollidable);
            }
            else
            {
                throw new InvalidCastException("Failed to cast sender to ICollidable");
            }
        }

        private void checkCollision(ICollidable i_Source)
        {
            if (i_Source.Visible && this.Enabled)
            {
                for (int i = 0; i < r_Collidables.Count - 1; i++)
                {
                    ICollidable target = r_Collidables[i];
                    if (i_Source != target && target.Visible && target.Enabled)
                    {
                        if (target.CheckCollision(i_Source))
                        {
                            target.Collided(i_Source);
                            i_Source.Collided(target);
                        }
                    }
                }
            }
        }
        
        public class ObjectDuplicateException : Exception
        {
            public new string Message { get; private set; }

            public ObjectDuplicateException(object i_Object, IEnumerable i_Collection)
                : base()
            {
                Message = string.Format
                    ("Trying to add an existing object to collection (Duplicate):/n{0}/n{1}", i_Object.ToString(), i_Collection.ToString());
            }
        }
    }
}
