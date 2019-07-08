namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Ex03.SpaceInvaders.Library.Sprites;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.SpaceInvaders.Library.Managers;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.GameServices;
    using Ex03.Infrastracture.Managers;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;
    using Ex03.SpaceInvaders.Library.Sprites.Bullets;

    public class PlayScreen : GameScreen, IPlayScreen
    {
        public int CurrentPlayerCount 
        { 
            get { return r_Crafts.Count; } 
        }

        private readonly List<IGameComponent> r_NonReusableComponents;
        private readonly List<SpaceCraft> r_Crafts;
        private MotherShip m_MotherShip;
        private ICollisionManager m_CollisionManager;
        private InvaderManager m_InvaderManager;
        private BarrierManager m_BarrierManager;
        private PauseScreen m_PauseScreen;

        public PlayScreen(Game i_Game)
            : base(i_Game)
        {
            r_Crafts = new List<SpaceCraft>();
            r_NonReusableComponents = new List<IGameComponent>();

            this.BlendState = BlendState.NonPremultiplied;

            this.Game.Services.AddService(typeof(IPlayScreen), this);

            m_CollisionManager = new CollisionManager(this.Game);

            m_PauseScreen = new PauseScreen(this.Game);

            createGameComponents();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.InputManager.KeyPressed(Keys.P))
            {
                this.ScreenManager.SetCurrentScreen(m_PauseScreen);
            }

            if (this.InputManager.KeyPressed(Keys.Escape))
            {
                this.Game.Exit();
            }

            base.Update(i_GameTime);
        }

        public override void Add(IGameComponent i_Component)
        {
            base.Add(i_Component);

            SpaceCraft craft = i_Component as SpaceCraft;
            if (craft != null)
            {
                r_Crafts.Add(craft);
                craft.EnabledChanged += spaceCraft_EnabledChanged;
            }
            else if (i_Component is BulletSpawner || i_Component is SoulSprite || i_Component is ScoreBoard)
            {
                r_NonReusableComponents.Add(i_Component);
            }
        }

        public void ResetNewGame()
        {
            clearNonReusableComponents();

            this.Add(new SpaceCraft(this.Game, @"Sprites\Ships_64x32"));

            m_MotherShip.Visible = false;

            m_InvaderManager.ResetNewGame();

            m_BarrierManager.ResetNextLevel(0);
        }

        public void ResetNextLevel(int i_Level)
        {
            int remain = i_Level % 4;

            m_CollisionManager.Enabled = false;

            m_MotherShip.Visible = false;

            m_InvaderManager.ResetNextLevel(remain);

            m_BarrierManager.ResetNextLevel(remain);

            m_CollisionManager.Enabled = true;
        }

        protected override void ExecuteOnFirstRun()
        {
            base.ExecuteOnFirstRun();
            m_InvaderManager.OutOfScreenBoundries += m_InvaderManager_OutOfScreenBoundries;
        }

        private void m_InvaderManager_OutOfScreenBoundries()
        {
            this.ExitScreen();
        }

        protected override void ExecuteOnReset()
        {
            ResetNewGame();
        }

        protected override void InitiateScreenBoundries()
        {
            m_BarrierManager.InitiateBarrierLine();
            foreach (SpaceCraft craft in r_Crafts)
            {
                craft.InitBounds();
            }
        }

        private void clearNonReusableComponents()
        {
            foreach (IGameComponent component in r_NonReusableComponents)
            {
                this.Remove(component);
            }

            r_NonReusableComponents.Clear();
        }

        private void createGameComponents()
        {
            this.Add(m_BarrierManager = new BarrierManager(this.Game, 4));
            this.Add(new SpaceCraft(this.Game, @"Sprites\Ships_64x32"));
            this.Add(m_MotherShip = new MotherShip(this.Game, @"Sprites\MotherShip_32x120", Color.Red, 800));
            this.Add(m_InvaderManager = new InvaderManager(this.Game, new Vector2(9, 5)));
        }

        private void spaceCraft_EnabledChanged(object sender, EventArgs e)
        {
            r_Crafts.Remove(sender as SpaceCraft);
            this.Remove(sender as IGameComponent);
            if (r_Crafts.Count == 0)
            {
                this.ExitScreen();
            }
        }
    }
}
