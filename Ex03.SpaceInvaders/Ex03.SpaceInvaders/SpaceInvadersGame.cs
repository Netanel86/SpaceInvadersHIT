using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Ex03.Infrastracture.Managers;
using Ex03.Infrastracture.ServiceInterfaces;
using Ex03.SpaceInvaders.Library;
using Ex03.SpaceInvaders.Library.Sprites;
using Ex03.SpaceInvaders.Library.Sprites.Entities;
using Ex03.SpaceInvaders.Library.Sprites.Bullets;
using Ex03.SpaceInvaders.Library.GameServices;
using Ex03.SpaceInvaders.Library.Managers;
using Ex03.SpaceInvaders.Library.Screens;

namespace Ex03.SpaceInvaders
{
    public class SpaceInvadersGame : Game
    {
        private ScreenManager m_ScreenManager;
        private GraphicsDeviceManager m_Graphics;
        private InputManager m_InputManager;
        private GameProgressManager m_GameManager;

        private GameOverScreen m_GameOverScreen;
        private PlayScreen m_PlayScreen;
        private MainMenu m_MenuScreen;

        private SpaceInvadersAudioManager m_SpaceInvadersAudioManager;
        private AudioEngine m_AudioEngine;
        private WaveBank m_WaveBank;
        private SoundBank m_SoundBank;

        public SpaceInvadersGame()
            : base()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            m_Graphics.IsFullScreen = false;
            m_Graphics.PreferredBackBufferHeight = 768;
            m_Graphics.PreferredBackBufferWidth = 1366;

            this.Components.Add(m_InputManager = new InputManager(this));
            this.Components.Add(m_GameManager = new GameProgressManager(this));
            m_ScreenManager = new ScreenManager(this);
            this.Components.Add(new Background(this, @"Sprites\BG_Space01_1024x768"));

            m_PlayScreen = new PlayScreen(this);
            m_MenuScreen = new MainMenu(this);

            m_ScreenManager.Push(m_GameOverScreen = new GameOverScreen(this, m_PlayScreen, m_MenuScreen));
            m_ScreenManager.Push(m_PlayScreen);
            m_ScreenManager.Push(new LevelTransitionScreen(this, 0));
            m_ScreenManager.SetCurrentScreen(new WelcomeScreen(this, m_MenuScreen));

            m_AudioEngine = new AudioEngine(@"Content\Audio\xACTSpaceInvaders.xgs");
            m_WaveBank = new WaveBank(m_AudioEngine, @"Content\Audio\WaveBank.xwb");
            m_SoundBank = new SoundBank(m_AudioEngine, @"Content\Audio\SoundBank.xsb");
            m_SpaceInvadersAudioManager = new SpaceInvadersAudioManager(this, m_AudioEngine, m_WaveBank, m_SoundBank);
        }

        protected override void Initialize()
        {
            this.Services.AddService(typeof(SpriteBatch), new SpriteBatch(GraphicsDevice));
            Invader.BulletSpawnRate = 0.05f;
            Invader.BulletMax = 1;
            InvaderBullet.SurvivalChance = 50;

            this.IsMouseVisible = true;

            m_SpaceInvadersAudioManager.Play("BGMusic");

            base.Initialize();
        }
   }
}