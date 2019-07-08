namespace Ex03.SpaceInvaders.Library.Sprites.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Ex03.Infrastracture;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.GameServices;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public abstract class Enemy : Sprite, IInterceptable
    {
        public event EventHandler<EventArgs> Intercepted;

        protected IAudioManager m_AudioManager;

        public int Score { get; set; }

        public Enemy(Game i_Game, string i_AssetName)
            : base(i_Game, i_AssetName)
        {
        }
       
        public Enemy(Game i_Game, string i_AssetName, int i_ScoreHit)
            : base(i_Game, i_AssetName)
        {
            this.Score = i_ScoreHit;
        }

        public Enemy(Game i_Game, string i_AssetName, Color i_Color, int i_ScoreHit)
            : this(i_Game, i_AssetName, i_ScoreHit)
        {
            m_TintColor = i_Color;
        }

        public override void Initialize()
        {
            base.Initialize();

            m_AudioManager = Game.Services.GetService(typeof(IAudioManager)) as IAudioManager;
        }
        
        protected override void AutoGameServicesRegistration()
        {
            base.AutoGameServicesRegistration();

            IInterceptionManager gameManager = this.Game.Services.GetService(typeof(IInterceptionManager)) as IInterceptionManager;
            if (gameManager != null)
            {
                gameManager.AttachInterceptable(this);
            }
        }

        protected virtual void OnIntercepted(EventArgs i_Args)
        {
            if (Intercepted != null)
            {
                Intercepted.Invoke(this, i_Args);
            }
        }
    }
}
