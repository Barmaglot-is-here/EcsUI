using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EcsUI.Systems;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace EcsUI.Runtime;

public class Game1 : Game
{
    private const string MARKUP_PATH = "Markup\\TestMarkup.xml";

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private EcsWorld _ecsWorld;
    private EcsSystems _drawSystems;

    public Game1()
    {
        _graphics               = new GraphicsDeviceManager(this);
        Content.RootDirectory   = "Content";
        IsMouseVisible          = true;
    }

    private void InitECS()
    {
        _ecsWorld = new();
        var fonts = LoadFonts();

        UIParser UIParser = new(_graphics.GraphicsDevice, fonts);
        UIParser.Parse(MARKUP_PATH, _ecsWorld);

        _drawSystems = new(_ecsWorld, _spriteBatch);
        _drawSystems.Add(new InitPositionSystem())
                    .Add(new DarwTextSystem())
                    .Add(new DrawSpriteSystem())
                    .Init();
    }

    private Dictionary<string, SpriteFont> LoadFonts()
    {
        Dictionary<string, SpriteFont> fonts = new();

        var name = "Arial";
        var font = Content.Load<SpriteFont>(name);

        fonts.Add(name, font);

        name = "Comic Sans";
        font = Content.Load<SpriteFont>(name);

        fonts.Add(name, font);

        return fonts;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        InitECS();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Q))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.D))
            DisableText();
        if (Keyboard.GetState().IsKeyDown(Keys.E))
            EnableText();

        base.Update(gameTime);
    }

    private void DisableText()
    {
        int[] entities = null;

        _ecsWorld.GetAllEntities(ref entities);

        var enabledpool = _ecsWorld.GetPool<EnabledComponent>();
        var textPool = _ecsWorld.GetPool<TextComponent>();

        foreach (int entity in entities)
        {
            if (!textPool.Has(entity))
                continue;

            if (!enabledpool.Has(entity))
            {
                Debug.WriteLine("Already disabled");

                continue;
            }

            ref TextComponent text = ref textPool.Get(entity);

            enabledpool.Del(entity);
        }
    }

    private void EnableText()
    {
        int[] entities = null;

        _ecsWorld.GetAllEntities(ref entities);

        var enabledpool = _ecsWorld.GetPool<EnabledComponent>();
        var textPool = _ecsWorld.GetPool<TextComponent>();

        foreach (int entity in entities)
        {
            if (!textPool.Has(entity))
                continue;

            if (enabledpool.Has(entity))
            {
                Debug.WriteLine("Already enabled");

                continue;
            }

            ref TextComponent text = ref textPool.Get(entity);

            enabledpool.Add(entity);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _drawSystems.Run();

        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
