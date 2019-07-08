namespace Ex03.Infrastracture.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Ex03.Infrastracture.ServiceInterfaces;

    public abstract class LoadableDrawableComponent : DrawableGameComponent
    {
        public event EventHandler<EventArgs> PositionChanged;
        
        public event EventHandler<EventArgs> SizeChanged;

        public string AssetName 
        { 
            get { return m_AssetName; } 

            set { m_AssetName = value; } 
        }

        protected string m_AssetName;

        protected ContentManager ContentManager 
        { 
            get { return this.Game.Content; } 
        } 

        public LoadableDrawableComponent(Game i_Game, string i_AssetName)
            : base(i_Game)
        {
            m_AssetName = i_AssetName;
        }

        public LoadableDrawableComponent(Game i_Game, string i_AssetName, int i_UpdateOrder, int i_DrawOrder)
            : this(i_Game, i_AssetName)
        {
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            InitBounds();
            
            AutoGameServicesRegistration();
        }

        /// <summary>
        /// User overridable method for initiating sprite bounds
        /// </summary>
        public abstract void InitBounds();

        /// <summary>
        /// User overridable method for adding on creation registration to game services
        /// </summary>
        protected virtual void AutoGameServicesRegistration()
        {
            if (this is ICollidable)
            {
                ICollisionManager collisionMgr =
                    this.Game.Services.GetService(typeof(ICollisionManager))
                        as ICollisionManager;

                if (collisionMgr != null)
                {
                    collisionMgr.AddCollidable(this as ICollidable);
                }
            }
        }

        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null)
            {
                SizeChanged.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
