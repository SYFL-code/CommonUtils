#region using
using BepInEx;
using BepInEx.Logging;
using Expedition;
using HarmonyLib;
using HUD;
using JetBrains.Annotations;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using MoreSlugcats;
using Newtonsoft.Json.Linq;
using Noise;
using RainMeadow;
using RWCustom;
using SlugBase.Features;
using Steamworks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;
using static MonoMod.InlineRT.MonoModRule;
using static SlugBase.Features.FeatureTypes;
using static UnityEngine.Input;
using ObjType = AbstractPhysicalObject.AbstractObjectType;
#endregion
namespace Scrap;
[Obsolete("Scrap 废案")]
[EditorBrowsable(EditorBrowsableState.Never)]
internal class Zname2//Scrap2 废案2
{

    #region ParticleEffect

    //public class ParticleEffect
    //{
    //    private class Particle
    //    {
    //        public FContainer container;
    //        public Vector2 velocity;
    //        public Vector2 worldPosition;
    //        public LightSource? lightSource;
    //        public float timer = 0f;
    //        public bool isActive = true;
    //    }

    //    private List<Particle> particles = new List<Particle>();
    //    private FContainer masterContainer;

    //    public float pixelSize = 1.5f;
    //    private const float maxLifetime = 90f; // 90帧 
    //    private float currentLifetime = 0f;
    //    public bool Destroyed = false;

    //    private Vector2 targetWorldPos;
    //    private bool reverse;
    //    private Room room;

    //    public ParticleEffect(Room room, Vector2 targetWorldPos, bool reverse)
    //    {
    //        this.targetWorldPos = targetWorldPos;
    //        this.reverse = reverse;
    //        this.room = room;

    //        try
    //        {
    //            // 创建主容器
    //            masterContainer = new FContainer();
    //            Futile.stage.AddChild(masterContainer);

    //            int particleCount = UnityEngine.Random.Range(15, 30);

    //            for (int i = 0; i < particleCount; i++)
    //            {
    //                CreateParticle();
    //            }
    //        }
    //        catch (System.Exception e)
    //        {
    //            Log.LogError($"粒子效果初始化失败: {e.Message}");
    //            Destroy();
    //            throw;
    //        }
    //    }

    //    private void CreateParticle()
    //    {
    //        Particle particle = new Particle();
    //        particle.container = new FContainer();
    //        masterContainer.AddChild(particle.container);
    //        particle.container.alpha = 0f; // 初始透明
    //        particle.container.isVisible = true;

    //        // 生成随机粒子形状
    //        GenerateRandomParticle(particle.container);
    //        particle.container.rotation = 90 * UnityEngine.Random.Range(0, 4);

    //        // 初始化位置和速度
    //        InitializeParticlePosition(particle);

    //        particles.Add(particle);
    //    }

    //    private void InitializeParticlePosition(Particle particle)
    //    {
    //        Vector2 cameraPos = room.game.cameras[0].pos;

    //        float TWO_PT = Mathf.PI * 2f;
    //        if (reverse)
    //        {
    //            // 反向粒子：从远处向目标汇聚
    //            float angle = UnityEngine.Random.Range(0f, TWO_PT);
    //            float radius = Mathf.Sqrt(UnityEngine.Random.Range(30f * 30f, 300f * 300f));

    //            // 世界坐标位置
    //            particle.worldPosition = targetWorldPos + new Vector2(
    //                Mathf.Cos(angle),
    //                Mathf.Sin(angle)
    //            ) * radius;

    //            // 速度方向指向目标
    //            particle.velocity = (targetWorldPos - particle.worldPosition).normalized *
    //                               UnityEngine.Random.Range(radius / 100, radius / 50); // 初始较快速度
    //        }
    //        else
    //        {
    //            // 正向粒子：从目标向外扩散
    //            float angle = UnityEngine.Random.Range(0f, TWO_PT);
    //            float radius = UnityEngine.Random.Range(0f, 30f);

    //            // 世界坐标位置
    //            particle.worldPosition = targetWorldPos + new Vector2(
    //                Mathf.Cos(angle),
    //                Mathf.Sin(angle)
    //            ) * radius;

    //            // 速度方向向外扩散
    //            particle.velocity = (particle.worldPosition - targetWorldPos).normalized *
    //                               UnityEngine.Random.Range(1f, 5f); // 初始较快速度
    //        }

    //        // 设置屏幕位置
    //        UpdateScreenPosition(particle, cameraPos);
    //    }

    //    public void Update()
    //    {
    //        if (Destroyed) return;

    //        Vector2 cameraPos = room.game.cameras[0].pos;
    //        Rect cameraRect = new Rect(
    //            cameraPos.x - 1000f,
    //            cameraPos.y - 1000f,
    //            Custom.rainWorld.options.ScreenSize.x + 2000f,
    //            Custom.rainWorld.options.ScreenSize.y + 2000f
    //        );

    //        bool anyActive = false;
    //        currentLifetime += 1f;

    //        for (int i = particles.Count - 1; i >= 0; i--)
    //        {
    //            Particle particle = particles[i];

    //            if (!particle.isActive) continue;

    //            // 更新粒子计时器
    //            particle.timer += 1f;

    //            // 屏幕外检测
    //            if (!cameraRect.Contains(particle.worldPosition))
    //            {
    //                RemoveParticle(particle);
    //                continue;
    //            }

    //            // 更新位置和速度
    //            UpdateParticlePosition(particle);

    //            // 更新屏幕位置
    //            UpdateScreenPosition(particle, cameraPos);

    //            // 更新光源
    //            UpdateLightSource(particle);

    //            // 更新透明度
    //            UpdateParticleAlpha(particle);

    //            anyActive = true;
    //        }

    //        // 检查是否所有粒子都已销毁
    //        if (!anyActive || currentLifetime >= maxLifetime)
    //        {
    //            Destroy();
    //        }
    //    }

    //    private void UpdateParticlePosition(Particle particle)
    //    {
    //        // 粒子生命周期阶段划分（基于计时器）
    //        if (particle.timer < 30f)
    //        {
    //            // 第一阶段：0-30帧 - 快速移动
    //            // 保持初始速度
    //        }
    //        else if (particle.timer < 40f)
    //        {
    //            // 第二阶段：30-40帧 - 减速
    //            float slowdownFactor = 1f - (particle.timer - 30f) / 10f; // 从1到0线性减速
    //            particle.velocity *= slowdownFactor;
    //        }
    //        else
    //        {
    //            // 第三阶段：40+帧 - 停止
    //            particle.velocity = Vector2.zero;
    //        }

    //        // 更新世界位置
    //        particle.worldPosition += particle.velocity;
    //    }

    //    private void UpdateScreenPosition(Particle particle, Vector2 cameraPos)
    //    {
    //        // 世界坐标转屏幕坐标
    //        Vector2 screenPos = new Vector2(
    //            particle.worldPosition.x - cameraPos.x,
    //            particle.worldPosition.y - cameraPos.y
    //        );

    //        particle.container.SetPosition(screenPos);
    //    }

    //    private void UpdateLightSource(Particle particle)
    //    {
    //        Vector2 cameraPos = room.game.cameras[0].pos;
    //        Vector2 screenPos = new Vector2(
    //            particle.worldPosition.x - cameraPos.x,
    //            particle.worldPosition.y - cameraPos.y
    //        );

    //        if (particle.lightSource == null)
    //        {
    //            // 使用正确的世界坐标创建光源
    //            particle.lightSource = new LightSource(
    //                screenPos,
    //                false,
    //                new Color(209f / 255f, 69f / 255f, 247f / 255f),
    //                null
    //            );

    //            if (room != null)
    //            {
    //                room.AddObject(particle.lightSource);
    //            }
    //        }
    //        else
    //        {
    //            // 更新光源位置（使用世界坐标）
    //            particle.lightSource.setPos = screenPos;
    //            particle.lightSource.requireUpKeep = true;

    //            // 设置光源半径（根据生命周期调整）
    //            float radius = 0f;
    //            if (particle.timer < 30f)
    //            {
    //                // 0-30帧：光源半径逐渐增大
    //                radius = 50f * (particle.timer / 30f);
    //            }
    //            else if (particle.timer < 40f)
    //            {
    //                // 30-40帧：保持最大半径
    //                radius = 50f;
    //            }
    //            else
    //            {
    //                // 40+帧：光源半径随透明度减小
    //                radius = 50f * particle.container.alpha;
    //            }

    //            particle.lightSource.setRad = radius;
    //            particle.lightSource.stayAlive = true;

    //            // 更新光源透明度
    //            particle.lightSource.setAlpha = particle.container.alpha;

    //            // 检查光源是否需要销毁
    //            if (particle.lightSource.slatedForDeletetion || particle.lightSource.room != room)
    //            {
    //                particle.lightSource.Destroy();
    //                particle.lightSource = null;
    //            }
    //        }
    //    }

    //    private void UpdateParticleAlpha(Particle particle)
    //    {
    //        if (particle.timer < 10f)
    //        {
    //            // 0-10帧：快速显示（淡入）
    //            particle.container.alpha = particle.timer / 10f;
    //        }
    //        else if (particle.timer < 40f)
    //        {
    //            // 10-40帧：保持完全可见
    //            particle.container.alpha = 1f;
    //        }
    //        else
    //        {
    //            // 40+帧：缓慢消失（淡出）
    //            float fadePhase = (particle.timer - 40f) / 40f;
    //            particle.container.alpha = Mathf.Max(0f, 1f - fadePhase);

    //            // 完全透明后标记为不活跃
    //            if (particle.container.alpha <= 0f)
    //            {
    //                particle.isActive = false;
    //            }
    //        }
    //    }

    //    private void RemoveParticle(Particle particle)
    //    {
    //        if (particle.container != null)
    //        {
    //            masterContainer.RemoveChild(particle.container);
    //            particle.container = null;
    //        }

    //        if (particle.lightSource != null)
    //        {
    //            particle.lightSource.Destroy();
    //            particle.lightSource = null;
    //        }

    //        particles.Remove(particle);
    //    }

    //    public void Destroy()
    //    {
    //        if (Destroyed) return;

    //        Destroyed = true;

    //        // 销毁所有粒子
    //        for (int i = particles.Count - 1; i >= 0; i--)
    //        {
    //            RemoveParticle(particles[i]);
    //        }

    //        // 销毁主容器
    //        if (masterContainer != null)
    //        {
    //            Futile.stage.RemoveChild(masterContainer);
    //            masterContainer = null;
    //        }
    //    }

    //    private void GenerateRandomParticle(FContainer container)
    //    {
    //        int shapeType = UnityEngine.Random.Range(0, 6);

    //        switch (shapeType)
    //        {
    //            case 0:
    //                CreatePixel(container, Vector2.zero);
    //                break;

    //            case 1:
    //                CreateShape1(container);
    //                break;

    //            case 2:
    //                CreateShape2(container);
    //                break;

    //            case 3:
    //                CreateShape3(container);
    //                break;

    //            default:
    //                CreatePixel(container, Vector2.zero);
    //                break;
    //        }
    //    }

    //    private void CreatePixel(FContainer container, Vector2 position)
    //    {
    //        FSprite pixel = new FSprite("pixel")
    //        {
    //            width = pixelSize,
    //            height = pixelSize,
    //            color = new Color(209f / 255f, 69f / 255f, 247f / 255f, 1f),
    //            x = position.x,
    //            y = position.y
    //        };
    //        container.AddChild(pixel);
    //    }

    //    private void CreateShape1(FContainer container)
    //    {
    //        Vector2[] positions = {
    //                new Vector2(0, 0),
    //                new Vector2(pixelSize, 0),
    //                new Vector2(pixelSize, pixelSize),
    //                new Vector2(pixelSize, pixelSize * 2),
    //                new Vector2(0, pixelSize * 2),
    //                new Vector2(-pixelSize, pixelSize * 2),
    //                new Vector2(-pixelSize * 2, pixelSize * 2),
    //                new Vector2(-pixelSize * 2, pixelSize),
    //                new Vector2(-pixelSize * 2, 0),
    //                new Vector2(-pixelSize * 2, -pixelSize),
    //                new Vector2(-pixelSize * 2, -pixelSize * 2),
    //                new Vector2(-pixelSize, -pixelSize * 2),
    //                new Vector2(0, -pixelSize * 2),
    //                new Vector2(pixelSize, -pixelSize * 2)
    //            };

    //        foreach (Vector2 pos in positions)
    //        {
    //            CreatePixel(container, pos);
    //        }
    //    }

    //    private void CreateShape2(FContainer container)
    //    {
    //        Vector2[] positions = {
    //                new Vector2(0, 0),
    //                new Vector2(pixelSize * 2, 0),
    //                new Vector2(-pixelSize * 2, 0),
    //                new Vector2(pixelSize, pixelSize),
    //                new Vector2(-pixelSize, pixelSize),
    //                new Vector2(pixelSize * 2, pixelSize * 2),
    //                new Vector2(-pixelSize * 2, pixelSize * 2),
    //                new Vector2(0, pixelSize * 2),
    //                new Vector2(pixelSize, -pixelSize),
    //                new Vector2(-pixelSize, -pixelSize),
    //                new Vector2(pixelSize * 2, -pixelSize * 2),
    //                new Vector2(-pixelSize * 2, -pixelSize * 2),
    //                new Vector2(0, -pixelSize * 2)
    //            };

    //        foreach (Vector2 pos in positions)
    //        {
    //            CreatePixel(container, pos);
    //        }
    //    }

    //    private void CreateShape3(FContainer container)
    //    {
    //        Vector2[] positions = {
    //				// 中心点
    //				new Vector2(0, 0),

    //				// 45° 对角线散射
    //				new Vector2(pixelSize, pixelSize),
    //                new Vector2(-pixelSize, -pixelSize),
    //                new Vector2(pixelSize, -pixelSize),
    //                new Vector2(-pixelSize, pixelSize),

    //				// 横向纵向扩展
    //				new Vector2(pixelSize * 2, 0),
    //                new Vector2(-pixelSize * 2, 0),
    //                new Vector2(0, pixelSize * 2),
    //                new Vector2(0, -pixelSize * 2),

    //				// 外层对角线大范围
    //				/*new Vector2(pixelSize * 3, pixelSize * 3),
    //				new Vector2(-pixelSize * 3, -pixelSize * 3),
    //				new Vector2(pixelSize * 3, -pixelSize * 3),
    //				new Vector2(-pixelSize * 3, pixelSize * 3)*/
    //			};

    //        foreach (Vector2 pos in positions)
    //        {
    //            CreatePixel(container, pos);
    //        }
    //    }


    //}

    #endregion

    #region EnderPearl()

    //public EnderPearl(EnderPearlAbstract abstr, Vector2 pos) : base(abstr, abstr.world)
    //{
    //	Abstr = abstr;

    //	bodyChunks = new BodyChunk[1];
    //	bodyChunks[0] = new BodyChunk(this, 0, pos, 6, 0.04f);

    //	//bodyChunks = new[] { new BodyChunk(this, 0, pos + vel, 4 * (Abstr.scaleX + Abstr.scaleY), 0.35f) { goThroughFloors = true } };
    //	bodyChunks[0].lastPos = bodyChunks[0].pos;

    //	firstChunk.loudness = 9f; // 响度
    //	firstChunk.pos = pos;

    //	bodyChunkConnections = new BodyChunkConnection[0];

    //	gravity = 0.65f;  // 重力
    //	airFriction = 0.999f; // 空气摩擦
    //	waterFriction = 0.92f;  // 水摩擦
    //	surfaceFriction = 0.1f;   // 表面摩擦力
    //	bounce = 0.1f;   // 弹力
    //	buoyancy = 0.35f;  // 浮力
    //	collisionLayer = 2;      // 层碰撞

    //	rotation = Rand * 360f;
    //	lastRotation = rotation;
    //	rotVel = 0f;

    //	rotationOffset = Rand * 30 - 15;

    //	rotationOffset = Rand * 30 - 15; // 旋转偏移
    //	ResetVel(vel.magnitude); // 重置速度(速度.向量的长度)

    //	// 初始化物理对象的身体块（BodyChunk）数组，这里只包含一个块
    //	base.bodyChunks = new BodyChunk[1];
    //	// 创建第一个身体块，设置位置为(0,0)，质量为5，弹性系数为0.07
    //	base.bodyChunks[0] = new BodyChunk(this, 0, new Vector2(0f, 0f), 5f, 0.07f);

    //	// 初始化身体块之间的连接数组，这里为空（没有连接）
    //	this.bodyChunkConnections = new PhysicalObject.BodyChunkConnection[0];

    //	// 设置物理属性
    //	base.airFriction = 0.999f;  // 空气阻力系数
    //	base.gravity = 0.9f;        // 重力强度
    //	this.bounce = 0.4f;         // 反弹系数
    //	this.surfaceFriction = 0.4f; // 表面摩擦力
    //	this.collisionLayer = 2;     // 碰撞层级（2表示特定碰撞层）

    //	// 设置水中物理属性
    //	base.waterFriction = 0.98f;  // 水中阻力系数
    //	base.buoyancy = 0.4f;        // 浮力系数

    //	// 控制物理行为的布尔标志
    //	this.pivotAtTip = false;     // 是否以尖端为旋转支点
    //	this.lastPivotAtTip = false; // 上一帧的支点状态
    //	this.stuckBodyPart = -1;     // 当前未附着任何身体部位（-1表示无）

    //	// 设置第一个身体块的音量（可能用于音效）
    //	base.firstChunk.loudness = 7f;
    //	// 初始化尾部位置为第一个身体块的位置
    //	this.tailPos = base.firstChunk.pos;

    //	// 创建基于第一个身体块的动态声音循环
    //	this.soundLoop = new ChunkDynamicSoundLoop(base.firstChunk);

    //	// 初始化水平光束状态数组（长度为3）
    //	this.wasHorizontalBeam = new bool[3];

    //	// 以下是与"Spearmaster"（可能为某种武器/角色）相关的属性
    //	this.spearmasterNeedle = false;  // 是否激活"Spearmaster"的针状物
    //	this.spearmasterNeedle_hasConnection = false; // 针状物是否有连接
    //	this.spearmasterNeedle_fadecounter_max = 400; // 针状物淡出时间的最大值
    //	this.spearmasterNeedle_fadecounter = this.spearmasterNeedle_fadecounter_max; // 当前淡出计时器
    //	this.spearmasterNeedleType = UnityEngine.Random.Range(0, 3); // 随机选择针状物类型（0-2）

    //	// 自定义颜色（未设置，默认为null）
    //	this.jollyCustomColor = null;

    //}

    #endregion

    #region ParticleEffect1
    /*using RWCustom;
using System.Diagnostics;
using System;
using Watcher;
using UnityEngine;
using System.Collections.Generic;


namespace EnderPearl
{
	public class ParticleEffect1 : CosmeticSprite
	{
		public static string[] ParticleName = { "Particle1", "Particle2", "Particle3", "Particle4", "Particle5", "Particle6", "Particle7", "Particle8", "Particle9" };
		public static string[] snowName = { "snow1", "snow2", "snow3", "snow4", "snow5", "snow6", "snow7" };

		private float lifeTime;
		private float lifeTime_max = 20;
		private float disappearTime = 0.2f;
		private LightSource? lightSource = null;

		private Vector2 CenterPos;
		private Vector2 direction;

		private bool reverse = false;//反转雪花运动 向目标中心汇聚

		//参数
		//public delegate Vector2 GetTracePos_Snow(Player self);
		//private GetTracePos_Snow? getTracePos = null;
		//private Player? getTracePos_arg1;

		public static void HookTexture()
		{
			//加载雪花贴图
			Futile.atlasManager.LoadAtlas("atlases/snow1");
			Futile.atlasManager.LoadAtlas("atlases/snow2");
			Futile.atlasManager.LoadAtlas("atlases/snow3");
			Futile.atlasManager.LoadAtlas("atlases/snow4");
			Futile.atlasManager.LoadAtlas("atlases/snow5");
			Futile.atlasManager.LoadAtlas("atlases/snow6");
			Futile.atlasManager.LoadAtlas("atlases/snow7");
			//加载贴图
			*//*Futile.atlasManager.LoadAtlas("atlases/Particle1");
			Futile.atlasManager.LoadAtlas("atlases/Particle2");
			Futile.atlasManager.LoadAtlas("atlases/Particle3");
			Futile.atlasManager.LoadAtlas("atlases/Particle4");
			Futile.atlasManager.LoadAtlas("atlases/Particle5");
			Futile.atlasManager.LoadAtlas("atlases/Particle6");
			Futile.atlasManager.LoadAtlas("atlases/Particle7");
			Futile.atlasManager.LoadAtlas("atlases/Particle8");
			Futile.atlasManager.LoadAtlas("atlases/Particle9");*//*
		}

		public ParticleEffect1(Vector2 setPos, bool reverse)
		{
			this.reverse = reverse;
			this.CenterPos = setPos;

			if (reverse)
			{
				// 修改1：在远处生成粒子（离中心位置较远）
				float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
				float distance = UnityEngine.Random.Range(100f, 300f); // 加大距离确保从外向内

				pos = setPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

				// 修改2：反转方向计算逻辑（使用 pos - setPos 来获得正确的向内速度）
				direction = (CenterPos - pos).normalized;
				vel = direction * UnityEngine.Random.Range(15f, 25f); // 适当提高速度
			}
			else
			{
				// 保持原来的放射状逻辑
				Vector2 vector;
				vector.x = UnityEngine.Random.Range(-1.5f, 0f) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
				vector.y = UnityEngine.Random.Range(-1.5f, 0f) * (UnityEngine.Random.Range(0, 2) * 2 - 1);

				pos = setPos + vector;
				direction = (CenterPos - pos).normalized;
				vel = -direction * UnityEngine.Random.Range(5f, 20f);
			}

			lifeTime = lifeTime_max;
			lastPos = pos;  // 修正：初始化 lastPos 为当前 pos

			*//*this.reverse = reverse;

			Vector2 vector;
			if (reverse)
			{
				vector.x = UnityEngine.Random.Range(-20f, 20f);
				vector.y = UnityEngine.Random.Range(-20f, 20f);
			}
			else
			{
				vector.x = UnityEngine.Random.Range(-1.5f, 0f) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
				vector.y = UnityEngine.Random.Range(-1.5f, 0f) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
			}

			//雪花
			CenterPos = setPos;
			//从周围随机半径角度产生雪花
			pos = setPos;
			pos += vector;

			direction = (setPos - pos).normalized;

			if (reverse)
			{
				vel = direction * UnityEngine.Random.Range(5f, 20f);
			}
			else
			{
				vel = -direction * UnityEngine.Random.Range(5f, 20f);
			}

			lifeTime = lifeTime_max;
			lastPos = setPos;*//*
		}

		*//*public ParticleEffect(Vector2 pos, Vector2 decVel, bool reverse = false)
		{
			this.reverse = reverse;
			lifeTime = lifeTime_max;
			this.pos = pos;
			lastPos = pos;
			vel.x = UnityEngine.Random.Range(-10f, 10f);
			vel.y = UnityEngine.Random.Range(-10f, 10f);
			vel -= decVel;
		}*//*

		public static string GetParticleStringRandom()
		{
			return snowName[UnityEngine.Random.Range(0, 6)];
			//return ParticleName[UnityEngine.Random.Range(0, 9)];
		}

		private readonly CustomAtlases customAtlases;

		private FAtlas ParticleAtlas;
		private FSprite[] sprites;

		public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
		{
			string ParticleString = GetParticleStringRandom();

			this.ParticleAtlas = this.customAtlases.LoadAtlases(ParticleString);

			sLeaser.sprites = new FSprite[1];
			this.sprites = sLeaser.sprites;

			sLeaser.sprites[0] = (this.sprites[0] = new FSprite(ParticleString, true));
			//sLeaser.sprites[0] = new FSprite(GetParticleStringRandom());
			FContainer fcontainer = rCam.ReturnFContainer("HUD");
			fcontainer.AddChild(sLeaser.sprites[0]);
		}

		public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
		{
			base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
			Vector2 v = Vector2.Lerp(lastPos, pos, timeStacker);
			v -= Vector2.Lerp(rCam.lastPos, rCam.pos, timeStacker);
			sLeaser.sprites[0].x = v.x;
			sLeaser.sprites[0].y = v.y;
			if (reverse)
				sLeaser.sprites[0].alpha = (lifeTime_max - lifeTime) / (lifeTime_max * disappearTime);
			else
				sLeaser.sprites[0].alpha = lifeTime / (lifeTime_max * disappearTime);
		}

		*//*private bool DisappearDistance()
		{
			//Vector2 dstPos = getTracePos(getTracePos_arg1);
			Vector2 dstPos = CenterPos;
			return (dstPos - pos).sqrMagnitude <= Mathf.Max(1.0f, vel.sqrMagnitude);
		}*//*

		private void Disappear()
		{
			if (lightSource != null)
				lightSource.Destroy();
			Destroy();
		}

		public override void Update(bool eu)
		{
			base.Update(eu);

			*//*bool ad = reverse && getTracePos != null;
			if (ad)
			{
				//改变能量方向
				Vector2 dstPos = getTracePos(getTracePos_arg1);
				//追踪坐标
				Vector2 v1 = (dstPos - pos).normalized;
				vel = vel.magnitude * v1;
			}*//*

			//vel *= 0.75f * lifeTime / lifeTime_max;

			if (lightSource == null)
			{
				lightSource = new LightSource(pos, false, new Color(209f / 255f, 69f / 255f, 247f / 255f), null);
				room.AddObject(lightSource);
			}
			if (lightSource != null)
			{
				lightSource.requireUpKeep = true;
				lightSource.setPos = pos;
				if (reverse)
					lightSource.setRad = (lifeTime_max - lifeTime) * 50f / lifeTime_max;
				else
					lightSource.setRad = lifeTime * 50f / lifeTime_max;
				lightSource.stayAlive = true;
				lightSource.setAlpha = 1f;
				if (lightSource.slatedForDeletetion || lightSource.room != room)
					lightSource = null;
			}

			// 修改3：优化消失条件，添加距离检测
			float distanceToCenter = Vector2.Distance(pos, CenterPos);

			if (reverse)
			{
				// 当粒子接近中心时消失
				if (distanceToCenter < 10f || lifeTime <= 0) // 适当增加消失半径
				{
					Disappear();
				}
				else
				{
					// 增加向中心加速效果
					vel += (CenterPos - pos).normalized * 0.5f;
				}
			}
			else
			{
				if (lifeTime <= 0)
					Disappear();
			}

			lifeTime--;

			*//*if (lifeTime > 0)
				lifeTime--;
			//没到时间但是已经在距离内
			if (reverse && Vector2.Distance(pos, CenterPos) < 4)
				Disappear();
			if (lifeTime <= 0)
			{
				if (reverse)
				{
					//Disappear();
					if (Vector2.Distance(pos, CenterPos) < 4)
						Disappear();
					else
						vel *= 1.1f;
				}
				else
					Disappear();
			}*//*
		}





	}
}*/

    #endregion

    #region CustomAtlases自定义地图集
    /*using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EnderPearl
{
	public class CustomAtlases
	{

		public FAtlas LoadAtlases(string imageName)
		{
			Texture2D? texture = this.LoadTexture(imageName);
			return Futile.atlasManager.LoadAtlasFromTexture(imageName, texture, false);
		}


		public Texture2D? LoadTexture(string imageName)
		{
			this.pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			this.assetsPath = Path.Combine(this.pluginPath, "../assets");
			string text = Path.Combine(Application.streamingAssetsPath, this.assetsPath, imageName + ".png");
			bool flag = File.Exists(text);
			if (flag)
			{
				byte[] data = File.ReadAllBytes(text);
				Texture2D texture2D = new Texture2D(2, 2);
				bool flag2 = texture2D.LoadImage(data);
				if (flag2)
				{
					texture2D.filterMode = FilterMode.Point;
					return texture2D;
				}
				Debug.LogError("Failed to load texture from file: " + text);
			}
			else
			{
				Debug.LogError("File not found at path: " + text);
			}
			return null;
		}


		private string? pluginPath;


		private string? assetsPath;
	}
}*/

    #endregion

    #region Cloudtail 云尾

    //UpdateRegurgitationGraphics
    //SlugcatHand_Update


    /*
	/// <summary>
	/// Cloudtail（云尾）自定义角色模块
	/// 实现基于内存数组的临时物品/生物吞咽-吐出仓储系统
	/// </summary>
	public static class CloudtailModule
	{
		/// <summary>
		/// 判断指定玩家是否为 Cloudtail 角色
		/// (通过 SlugBase 的角色类名识别)
		/// </summary>
		public static bool IsCloud(this Player pl)
		{
			return pl.SlugCatClass.value == "Cloudtail";
		}

		/// <summary>
		/// 腹中仓储容量（最多同时存储 5 个物体）
		/// </summary>
		public static readonly int capacity = 5;

		/// <summary>
		/// 条件弱引用表，将 CloudData 实例安全绑定到 Player 对象上
		/// 当 Player 被 GC 回收时，关联的 CloudData 自动清理，无需手动管理
		/// </summary>
		private static readonly ConditionalWeakTable<Player, CloudData> CloudCWT = new ConditionalWeakTable<Player, CloudData>();

		/// <summary>
		/// 获取玩家的 CloudData（懒初始化）
		/// 首次访问时自动创建并绑定
		/// </summary>
		public static CloudData GetCloudData(this Player player)
		{
			return CloudCWT.GetValue(player, (Player _) => new CloudData(player));
		}

		/// <summary>
		/// 安全获取 CloudData
		/// 返回 true 且 out sailor 非空 仅当玩家是 Cloudtail
		/// 否则返回 false, sailor = null
		/// </summary>
		public static bool TryGetCloud(this Player player, out CloudData? sailor)
		{
			bool result;
			if (player.IsCloud())
			{
				sailor = player.GetCloudData();
				result = true;
			}
			else
			{
				sailor = null;
				result = false;
			}
			return result;
		}

		/// <summary>
		/// Cloudtail 角色的核心数据与行为类
		/// 每个 Cloudtail 玩家持有唯一实例，通过 ConditionalWeakTable 绑定
		/// </summary>
		public class CloudData
		{
			/// <summary>
			/// 构造函数，初始化仓储数组和计数器
			/// </summary>
			public CloudData(Player player)
			{
				self = player;
				Storage = new AbstractPhysicalObject[capacity]; // 容量 5
				SwallowOrRegurgitateCoutner = 0;                // 吞咽/呕吐长按计数器
				GraspToTakeFrom = -1;                            // 未被指定取物手
				Rolltimer = 0;
			}

			/// <summary> 所属玩家对象 </summary>
			public Player self;

			/// <summary>
			/// 存储数组，null 表示空槽位
			/// 仅存储抽象对象 (AbstractPhysicalObject)，不保留物理实现层
			/// </summary>
			public AbstractPhysicalObject?[] Storage;

			/// <summary> 记录触发吞咽时使用的“手”索引 (0或1) </summary>
			public int GraspToTakeFrom;

			/// <summary>
			/// 仓储是否已满
			/// (所有槽位均非空)
			/// </summary>
			public bool Stuffed
			{
				get
				{
					return Storage != null && FreeSlot() == -1;
				}
			}

			/// <summary>
			/// 是否具有漂浮效果
			/// (腹中含有能量细胞时启用)
			/// </summary>
			public bool Floaty;

			/// <summary>
			/// 吞咽/呕吐蓄力计数器
			/// 满足条件时每帧 +1，达到 90 帧触发动作
			/// </summary>
			public int SwallowOrRegurgitateCoutner;

			/// <summary> 翻滚计时器 (目前未使用) </summary>
			public int Rolltimer;

			/// <summary>
			/// 自定义存储按键检测
			/// 根据玩家编号使用不同按键配置 (P1 ~ P4)
			/// </summary>
			public bool StorageInput
			{
				get
				{
					int num = self.playerState.playerNumber;

					if (num == 0)
					{
						return Input.GetKey(CloudtailOptions.Instance.KeyCode1.Value);
					}
					else if (num == 1)
					{
						return Input.GetKey(CloudtailOptions.Instance.KeyCode2.Value);
					}
					else if (num == 2)
					{
						return Input.GetKey(CloudtailOptions.Instance.KeyCode3.Value);
					}
					else
					{
						return Input.GetKey(CloudtailOptions.Instance.KeyCode4.Value);
					}
				}
			}

			/// <summary>
			/// 紧急清空仓储：将所有存储物吐回世界
			/// 注意：方法名称为 Save 但实际作用是清空（非持久化）
			/// 典型调用时机：角色死亡、切换区域
			/// </summary>
			public void Save()
			{
				for (int i = 0; i < capacity; i++)
				{
					if (Storage[i] != null)
					{
						var stomach = Storage[i]!;

						// 重新绑定世界引用
						stomach.world = self.room.world;

						// 若为生物，需重新关联 AI 到当前世界
						if (stomach is AbstractCreature cr)
						{
							cr.abstractAI?.NewWorld(self.room.world);
						}

						// 将抽象实体放回房间并实体化
						self.room.abstractRoom.AddEntity(stomach);
						stomach.pos = self.room.GetWorldCoordinate(self.firstChunk.pos);
						stomach.RealizeInRoom();

						// 清空槽位
						Storage[i] = null;
					}
				}
			}

			/// <summary>
			/// 检查当前是否可以执行“吞入”操作
			/// 条件：
			///   1. 无移动输入 + 按下自定义键
			///   2. 未在水中过深 (submersion <= 0.5)
			///   3. 腹中仍有空位
			///   4. 手中有非食用品 (或普通物品)
			///   5. 至少一只手非空
			/// </summary>
			public bool CanStore()
			{
				// 无方向键移动 + 按下自定义存储键
				bool Storeinputs = self.input[0].x == 0 && self.input[0].y == 0 && StorageInput;

				if (self.room == null) return false;              // 房间不存在
				if (!Storeinputs) return false;                   // 未按下存储键
				if (self.firstChunk.submersion > 0.5f) return false; // 半身入水时禁止
				if (Stuffed) return false;                        // 腹中已满

				// 双手均空，无法吞入
				if (self.grasps[0] == null && self.grasps[1] == null) return false;

				int handnum = -1;

				// 检查手中物品
				for (int i = 0; i < self.grasps.Length; i++)
				{
					// 如果是可食物品，不允许吞入（因可直接吃，防止绕过食物系统）
					if (self.grasps[i] != null && self.grasps[i].grabbed is IPlayerEdible food && food.Edible)
						return false;

					// 记录第一个非空手的索引
					if (self.grasps[i] != null)
					{
						handnum = i;
						break;
					}
				}

				if (handnum == -1) return false;

				// 记住将要拿取物品的手
				GraspToTakeFrom = handnum;

				return true;
			}

			/// <summary>
			/// 检查当前是否可以执行“吐出”操作
			/// 条件：
			///   1. 按下投掷键
			///   2. 未在吃肉动作中 (eatMeat <= 20)
			///   3. 未按拾取键 / 跳跃键
			///   4. 未在水中过深
			///   5. 至少一只手空闲
			/// </summary>
			public bool CanRetrieve()
			{
				return self.input[0].thrw
					&& self.eatMeat <= 20
					&& !self.input[0].pckp
					&& !self.input[0].jmp
					&& self.firstChunk.submersion < 0.5f
					&& self.room != null
					&& self.FreeHand() != -1;
			}

			/// <summary>
			/// 每帧更新
			/// - 检测吞/吐条件，累积计数器
			/// - 达到 90 帧阈值时执行吞或吐
			/// - 更新 Floaty 状态 (是否含能量细胞)
			/// </summary>
			public void Update()
			{
				if (self.room == null) return; // 房间未就绪，跳过

				bool s = CanStore();    // 是否可吞
				bool r = CanRetrieve(); // 是否可吐

				// 满足任一条件，累加蓄力计数器
				if (r || s)
				{
					SwallowOrRegurgitateCoutner++;

					// 蓄力满 90 帧 (~1.5秒)，触发动作
					if (SwallowOrRegurgitateCoutner > 90)
					{
						if (s)
						{
							Swallow(GraspToTakeFrom);

							// 触发吞物动画效果
							if (self.graphicsModule != null)
								(self.graphicsModule as PlayerGraphics).swallowing = 20;
						}
						else
						{
							Regurgitate();
						}

						SwallowOrRegurgitateCoutner = 0; // 重置计数器
					}
				}
				else
				{
					// 条件不满足，立即归零（避免误触）
					SwallowOrRegurgitateCoutner = 0;
				}

				// 未在合成物品时，重置原生吞/吐计数器
				if (!self.craftingObject)
					self.swallowAndRegurgitateCounter = 0;

				// 检测是否含有能量细胞 (用于悬浮效果)
				Floaty = StorageContains(MoreSlugcatsEnums.AbstractObjectType.EnergyCell, false, null);
			}

			/// <summary>
			/// 更新吞咽/呕吐时的动画特效
			/// 让角色身体部件产生上下/前后波动，模拟反胃感
			/// </summary>
			public void UpdateRegurgitationGraphics(PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser)
			{
				// 蓄力超 15 帧开始眨眼
				if (SwallowOrRegurgitateCoutner > 15)
				{
					self.blink = 5;
				}

				// 计算波动强度 (num10) 和波动频率参数 (num11)
				float num10 = Mathf.InverseLerp(0f, 110f, (float)SwallowOrRegurgitateCoutner);
				float num11 = (float)SwallowOrRegurgitateCoutner / Mathf.Lerp(30f, 15f, num10);

				if (self.player.standing)
				{
					// === 站立姿态：身体呈上下垂直波动 ===
					sLeaser.sprites[0].y += Mathf.Sin(num11 * 3.1415927f * 2f) * num10 * 2f;
					sLeaser.sprites[1].y += -Mathf.Sin((num11 + 0.2f) * 3.1415927f * 2f) * num10 * 3f;
					sLeaser.sprites[3].y += Mathf.Sin(num11 * 3.1415927f * 2f) * num10 * 2f;
					sLeaser.sprites[9].y += Mathf.Sin(num11 * 3.1415927f * 2f) * num10 * 2f;
				}
				else
				{
					// === 非站立姿态 (匍匐/倒挂等)：加入水平方向波动 ===
					sLeaser.sprites[0].y += Mathf.Sin(num11 * 3.1415927f * 2f) * num10 * 3f;
					sLeaser.sprites[0].x += Mathf.Cos(num11 * 3.1415927f * 2f) * num10 * 1f;
					sLeaser.sprites[1].y += Mathf.Sin((num11 + 0.2f) * 3.1415927f * 2f) * num10 * 2f;
					sLeaser.sprites[1].x += -Mathf.Cos(num11 * 3.1415927f * 2f) * num10 * 3f;
					sLeaser.sprites[3].y += Mathf.Sin(num11 * 3.1415927f * 2f) * num10 * 3f;
					sLeaser.sprites[3].x += Mathf.Cos(num11 * 3.1415927f * 2f) * num10 * 1f;
					sLeaser.sprites[9].y += Mathf.Sin(num11 * 3.1415927f * 2f) * num10 * 3f;
					sLeaser.sprites[9].x += Mathf.Cos(num11 * 3.1415927f * 2f) * num10 * 1f;
				}
			}

			/// <summary>
			/// 执行吞入操作
			/// 将手中物品/生物的抽象对象存入 Storage 数组，并从世界中移除其物理对象
			/// </summary>
			/// <param name="grasp">要吞入的手的索引 (0 或 1)</param>
			public void Swallow(int grasp)
			{
				// 获取手中抓取的物理对象
				PhysicalObject abstractPhysicalObject = self.grasps[grasp].grabbed;

				// 寻找第一个空槽位
				int storeindex = FreeSlot();
				if (storeindex == -1) return; // 无空槽位，取消

				// ————— 特殊物品处理 —————

				// 矛：清除插墙计数器（防止吞入卡状态的矛）
				if (abstractPhysicalObject is Spear)
				{
					(abstractPhysicalObject as Spear).abstractSpear.stuckInWallCycles = 0;
				}

				// 带电电矛：不能吞入，触发强烈电击惩罚
				if (abstractPhysicalObject is ElectricSpear && (abstractPhysicalObject as ElectricSpear).abstractSpear.electricCharge > 0)
				{
					self.room.AddObject(new ZapCoil.ZapFlash(self.firstChunk.pos, 10f));
					self.room.PlaySound(SoundID.Zapper_Zap, self.firstChunk.pos, 1f, 1.5f + Random.value * 1.5f);

					// 若在水中，产生更强电击扩散
					if (self.Submersion > 0.5f)
					{
						self.room.AddObject(new UnderwaterShock(self.room, null, self.firstChunk.pos, 10, 800f, 2f, self, new Color(0.8f, 0.8f, 1f)));
					}

					self.Stun(200);                                        // 晕眩 200 帧
					self.room.AddObject(new CreatureSpasmer(self, false, 200)); // 身体抽搐
					return; // 终止吞入
				}

				// 水母：不能吞入，触发较弱电击惩罚
				if (abstractPhysicalObject is JellyFish)
				{
					self.room.PlaySound(SoundID.Centipede_Shock, self.firstChunk.pos);
					self.room.AddObject(new UnderwaterShock(self.room, self, self.firstChunk.pos, 10, 100f, 0f, null, new Color(0.8f, 0.8f, 1f)));
					self.room.AddObject(new CreatureSpasmer(self, false, 60));
					self.Stun(80);
					return;
				}

				// ————— 正常吞入流程 —————

				// 获取抽象对象（这是保存的关键）
				var ToSwallow = abstractPhysicalObject.abstractPhysicalObject;

				// 存入仓储数组
				Storage[storeindex] = ToSwallow;

				// 松开手
				self.ReleaseGrasp(grasp);

				// 从房间移除物理对象
				abstractPhysicalObject.abstractPhysicalObject.realizedObject.RemoveFromRoom();

				// 抽象化（保留抽象对象，销毁物理实体）
				abstractPhysicalObject.abstractPhysicalObject.Abstractize(self.abstractCreature.pos);

				// 从房间实体列表中移除
				abstractPhysicalObject.abstractPhysicalObject.Room.RemoveEntity(abstractPhysicalObject.abstractPhysicalObject.ID);

				// 身体小弹跳反馈
				BodyChunk mainBodyChunk = self.mainBodyChunk;
				mainBodyChunk.vel.y += 2f;

				// 播放吞物音效
				self.room.PlaySound(SoundID.Slugcat_Swallow_Item, self.mainBodyChunk);
			}

			/// <summary>
			/// 执行吐出操作
			/// LIFO (后进先出)：最后存入的物体优先吐出
			/// 若仓储为空但有饱食度，可消耗 1 饱食度随机生成物品
			/// 若两者皆空，产生晕眩惩罚
			/// </summary>
			public void Regurgitate()
			{
				// 从后往前查找最后一个非空槽位 (LIFO 栈顶)
				int index = ItemInQueue();
				AbstractPhysicalObject stomach;
				bool PluckedFromNowhere = false; // 是否为凭空变出的物品

				if (index == -1) // 仓储中没有存储物
				{
					// 角色胃中有正常食物点数 → 消耗饱食度凭空变物
					if (self.FoodInStomach >= 1)
					{
						stomach = RandItem(self);    // 从随机池中生成一个物品
						PluckedFromNowhere = true;   // 标记为凭空生成
					}
					else
					{
						// 胃和仓储皆空 → 空吐惩罚：晕眩 + 增加劳累值
						self.Stun(80);
						self.AerobicIncrease(12f);
						return;
					}
				}
				else
				{
					// 正常取出仓储中的抽象对象
					stomach = Storage[index];
				}

				// ————— 将抽象对象重新实体化回世界 —————

				// 重新绑定当前世界
				stomach.world = self.abstractCreature.world;

				// 如果是生物，重新绑定 AI 到当前世界
				if (stomach is AbstractCreature c)
				{
					c.abstractAI?.NewWorld(self.room.world);
				}

				// 重新加入房间抽象实体列表
				self.room.abstractRoom.AddEntity(stomach);

				// 设置生成位置（在角色位置）
				stomach.pos = self.abstractCreature.pos;

				// 实体化：根据抽象对象生成物理对象
				stomach.RealizeInRoom();

				// 消耗 1 点饱食度（如果是凭空变出）
				if (PluckedFromNowhere)
				{
					self.SubtractFood(1);
				}

				// ————— 物理反馈：从嘴部喷射出物体 —————

				Vector2 vector = self.bodyChunks[0].pos;             // 头部位置
				Vector2 a = Custom.DirVec(self.bodyChunks[1].pos, self.bodyChunks[0].pos); // 身体方向
				bool flag = false;

				// 若角色近乎直立且头在上方，调整喷出方向和位置
				if (Mathf.Abs(self.bodyChunks[0].pos.y - self.bodyChunks[1].pos.y) > Mathf.Abs(self.bodyChunks[0].pos.x - self.bodyChunks[1].pos.x)
					&& self.bodyChunks[0].pos.y > self.bodyChunks[1].pos.y)
				{
					vector += Custom.DirVec(self.bodyChunks[1].pos, self.bodyChunks[0].pos) * 5f;
					a *= -1f;
					a.x += 0.4f * (float)self.flipDirection;
					a.Normalize();
					flag = true;
				}

				// 硬设置物体位置和速度（模拟吐出）
				stomach.realizedObject.firstChunk.HardSetPosition(vector);
				stomach.realizedObject.firstChunk.vel = Vector2.ClampMagnitude(
					(a * 2f + Custom.RNV() * Random.value) / stomach.realizedObject.firstChunk.mass,
					6f
				);

				// 反冲：角色身体轻微后退
				self.bodyChunks[0].pos -= a * 2f;
				self.bodyChunks[0].vel -= a * 2f;

				// 头部抖动效果
				if (self.graphicsModule != null)
				{
					(self.graphicsModule as PlayerGraphics).head.vel += Custom.RNV() * Random.value * 3f;
				}

				// 口水/水滴特效 x3
				for (int i = 0; i < 3; i++)
				{
					self.room.AddObject(new WaterDrip(
						vector + Custom.RNV() * Random.value * 1.5f,
						Custom.RNV() * 3f * Random.value + a * Mathf.Lerp(2f, 6f, Random.value),
						false
					));
				}

				// 播放呕吐音效
				self.room.PlaySound(SoundID.Slugcat_Regurgitate_Item, self.mainBodyChunk);

				// 如果满足条件且有空手，自动抓住吐出的物体
				if (flag && self.FreeHand() > -1)
				{
					self.SlugcatGrab(stomach.realizedObject, self.FreeHand());
				}

				// 清空对应槽位（如果是正常吐出而非凭空变物）
				if (!PluckedFromNowhere)
				{
					Storage[index] = null;
				}
			}

			/// <summary>
			/// 计算腹中存储物带来的保暖效果
			/// 每多一盏灯笼 (Lantern)，保暖倍数 +1
			/// </summary>
			public float HeatSources()
			{
				float heat = RainWorldGame.DefaultHeatSourceWarmth; // 基础温暖值
				float num = 1; // 基础倍率

				if (Storage == null) return heat;

				// 统计灯笼数量
				for (int i = 0; i < capacity; i++)
				{
					if (Storage[i] != null && Storage[i].type == ObjType.Lantern)
						num++;
				}

				heat *= num; // 应用倍率
				return heat;
			}

			/// <summary>
			/// 查找第一个空槽位的索引
			/// 用于吞入时寻找存放位置
			/// </summary>
			/// <returns>空槽位索引，-1 表示已满</returns>
			public int FreeSlot()
			{
				if (Storage == null) return -1;

				for (int i = 0; i < capacity; i++)
				{
					if (Storage[i] == null) return i;
				}

				return -1; // 仓储已满
			}

			/// <summary>
			/// 查找最后一个非空槽位的索引 (LIFO 栈顶)
			/// 用于吐出时确定要取出的物体
			/// </summary>
			/// <returns>最后一个非空槽位的索引，-1 表示仓储为空</returns>
			public int ItemInQueue()
			{
				if (Storage == null) return -1;

				// 从后往前遍历，找到最后存入的物体
				for (int i = capacity - 1; i >= 0; i--)
				{
					if (Storage[i] != null) return i;
				}

				return -1; // 仓储为空
			}

			/// <summary>
			/// 检查腹中是否含有指定类型的物品或生物
			/// </summary>
			/// <param name="objType">物品的抽象对象类型</param>
			/// <param name="CheckForCreatures">false=检查物品 / true=检查生物</param>
			/// <param name="creatType">(仅 CheckForCreatures=true 时) 要匹配的生物类型</param>
			/// <returns>是否包含</returns>
			public bool StorageContains(ObjType objType, bool CheckForCreatures, CreatureTemplate.Type creatType)
			{
				if (!CheckForCreatures)
				{
					// 匹配物品类型
					for (int i = 0; i < Storage.Length; i++)
					{
						if (Storage[i]?.type == objType)
						{
							return true;
						}
					}
					return false;
				}
				else
				{
					// 匹配生物类型
					for (int i = 0; i < Storage.Length; i++)
					{
						if (Storage[i] != null && (Storage[i] as AbstractCreature)?.creatureTemplate.type == creatType)
						{
							return true;
						}
					}
					return false;
				}
			}

			/// <summary>
			/// 随机生成一个物品/生物（用于凭空变物能力）
			/// 按概率分布返回不同种类，并将消耗品标记为已消耗、不新鲜
			/// </summary>
			public static AbstractPhysicalObject RandItem(Player caller)
			{
				AbstractPhysicalObject abstractPhysicalObject;
				float value = Random.value; // 0~1 随机数

				// 被注释掉的代码：原本可能想生成 SlugNPC，现已移除

				// ——— 按概率区间生成不同物品/生物 ———
				// 概率分布由 if-else 区间的宽度决定

				if (value <= 0.32894737f) // ~32.9%
				{
					// 鞭炮草
					abstractPhysicalObject = new AbstractConsumable(caller.room.world, ObjType.FirecrackerPlant, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null);
				}
				else if (value <= 0.4276316f) // ~9.9%
				{
					// 闪光弹
					abstractPhysicalObject = new AbstractConsumable(caller.room.world, ObjType.FlareBomb, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null);
				}
				else if (value <= 0.5065789f) // ~7.9%
				{
					// 拾荒者炸弹
					abstractPhysicalObject = new AbstractConsumable(caller.room.world, ObjType.ScavengerBomb, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null);
				}
				else if (value <= 0.6118421f) // ~10.5%
				{
					// 水坚果
					abstractPhysicalObject = new WaterNut.AbstractWaterNut(caller.room.world, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null, false);
				}
				else if (value <= 0.6644737f) // ~5.3%
				{
					// Yeek 生物 (More Slugcats 特有)
					abstractPhysicalObject = new AbstractCreature(caller.room.world, StaticWorld.GetCreatureTemplate(MoreSlugcatsEnums.CreatureTemplateType.Yeek), null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID());
				}
				else if (value <= 0.7302632f) // ~6.6%
				{
					// 灯笼鼠
					abstractPhysicalObject = new AbstractCreature(caller.room.world, StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LanternMouse), null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID());
				}
				else if (value <= 0.79605263f) // ~6.6%
				{
					// 管虫
					abstractPhysicalObject = new AbstractCreature(caller.room.world, StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.TubeWorm), null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID());
				}
				else if (value <= 0.82894737f) // ~3.3%
				{
					// 粉球
					abstractPhysicalObject = new AbstractConsumable(caller.room.world, ObjType.PuffBall, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null);
				}
				else if (value <= 0.8486842f) // ~2.0%
				{
					// 数据珍珠 (Rivulet_stomach 类型)
					abstractPhysicalObject = new DataPearl.AbstractDataPearl(caller.room.world, ObjType.DataPearl, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null, MoreSlugcatsEnums.DataPearlType.Rivulet_stomach);
				}
				else if (value <= 0.9144737f) // ~6.6%
				{
					// 泡泡草
					abstractPhysicalObject = new BubbleGrass.AbstractBubbleGrass(caller.room.world, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), 1f, -1, -1, null);
				}
				else if (value <= 0.93421054f) // ~2.0%
				{
					// 孢子植物 (半数概率自发光)
					abstractPhysicalObject = new SporePlant.AbstractSporePlant(caller.room.world, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null, false, (double)Random.value < 0.5);
				}
				else if (value <= 0.46710527f) // BUG: 此区间在之前的范围外，实际上不会被执行 (~3.9% 但顺序错误)
				{
					// 监察者尸体 (不同颜色代表不同迭代器)
					Color color = new Color(1f, 0.8f, 0.3f);     // 默认金色 (五卵石)
					int ownerIterator = 1;
					if (Random.value <= 0.35f)
					{
						color = new Color(0.44705883f, 0.9019608f, 0.76862746f); // 青色
						ownerIterator = 0; // 仰望皓月
					}
					else if (Random.value <= 0.05f)
					{
						color = new Color(0f, 1f, 0f); // 绿色
						ownerIterator = 2;              // 无稽烦忧?
					}
					abstractPhysicalObject = new OverseerCarcass.AbstractOverseerCarcass(caller.room.world, null, caller.abstractPhysicalObject.pos, caller.room.game.GetNewID(), color, ownerIterator);
				}
				else if (value <= 0.4736842f) // ~0.7%
				{
					// 小针虫
					abstractPhysicalObject = new AbstractCreature(caller.room.world, StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.SmallNeedleWorm), null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID());
				}
				else if (value <= 0.9934211f) // ~52.0% (大量填充区间)
				{
					// 小蜈蚣
					abstractPhysicalObject = new AbstractCreature(caller.room.world, StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.SmallCentipede), null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID());
				}
				else if (value <= 0.79605263f) // BUG: 此区间同样不会被触发 (重复区间)
				{
					// Yeek (再次出现，原设计可能想覆盖更多概率，但顺序错导致死区)
					abstractPhysicalObject = new AbstractCreature(caller.room.world, StaticWorld.GetCreatureTemplate(MoreSlugcatsEnums.CreatureTemplateType.Yeek), null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID());
				}
				else if (value <= 0.8f) // ~0.7%
				{
					// 秃鹫面具 (5% 概率获得王鹫面具)
					abstractPhysicalObject = new VultureMask.AbstractVultureMask(caller.room.world, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), caller.abstractPhysicalObject.ID.RandomSeed, (double)Random.value <= 0.05);
				}
				else // 兜底 (~0.7%)
				{
					// 矛大师珍珠
					abstractPhysicalObject = new SpearMasterPearl.AbstractDataPearl(caller.room.world, ObjType.DataPearl, null, caller.room.GetWorldCoordinate(caller.firstChunk.pos), caller.room.game.GetNewID(), -1, -1, null, MoreSlugcatsEnums.DataPearlType.Spearmasterpearl);
				}

				// 如果是消耗品类型，标记为已消耗 + 不新鲜
				// (防止被当作可食物品吃下)
				if (AbstractConsumable.IsTypeConsumable(abstractPhysicalObject.type))
				{
					(abstractPhysicalObject as AbstractConsumable).isFresh = false;
					(abstractPhysicalObject as AbstractConsumable).isConsumed = true;
				}

				return abstractPhysicalObject;
			}
		}
	}

	public static class MainMechanicsCloudtail
	{
		/// <summary>
		/// 判断玩家是否满足“美食家”挂钩条件。如果是 Cloudtail 则直接返回 true，否则调用原方法。
		/// </summary>
		public static bool IsGourmandHook(Func<Player, bool> orig, Player self)
		{
			return self.IsCloud() || orig(self);
		}

		/// <summary>
		/// 判断玩家能否被其他生物吞下。Cloudtail 不能被吞下，其他情况按原逻辑处理。
		/// </summary>
		public static bool CanBeSwallowed(On.Player.orig_CanBeSwallowed orig, Player self, PhysicalObject obj)
		{
			return !self.IsCloud() && orig(self, obj);
		}

		/// <summary>
		/// 每次更新进食状态时触发。Cloudtail 在满足特定输入和空闲存储槽位时，会将手中的物品吞入存储（而不是正常进食）。
		/// </summary>
		public static void EatMeatUpdate(On.Player.orig_EatMeatUpdate orig, Player self, int g)
		{
			if (self.TryGetCloud(out var data))
			{
				// 有空槽位，且玩家按下“下”方向，且该抓握手中有物品
				if (data.FreeSlot() != -1 && self.input[0].x == 0 && self.input[0].y == -1 && self.grasps[g] != null)
				{
					self.eatMeat = 0;         // 阻止正常进食
					data.Swallow(g);          // 触发吞咽存储
				}
			}

			orig(self, g);
		}

		/// <summary>
		/// 玩家构造函数。若为 Cloudtail，则额外初始化一条舌头（用于抓取/绳索机制）。
		/// </summary>
		public static void ctor(On.Player.orig_ctor orig, Player self, AbstractCreature creat, World world)
		{
			orig(self, creat, world);
			if (self.IsCloud())
			{
				self.tongue = new Player.Tongue(self, 0);
			}
		}

		/// <summary>
		/// 避难所门关闭时调用。遍历所有玩家，若 Cloudtail 存活且拥有存储数据，则执行保存操作。
		/// </summary>
		public static void Shelter_Close(On.ShelterDoor.orig_DoorClosed orig, ShelterDoor self)
		{
			for (int i = 0; i < self.room.game.Players.Count; i++)
			{
				if (self.room.game.Players[i].state.alive && self.room.game.Players[i].realizedCreature != null)
				{
					Player player = self.room.game.Players[i].realizedCreature as Player;
					if (player.TryGetCloud(out var data) && data.Storage != null)
					{
						data.Save();
					}
				}
			}
			orig(self);
		}

		/// <summary>
		/// 舌头缩短绳索长度时调用。Cloudtail 的舌头缩短速度受疲劳状态影响：极度疲劳时无法缩短，否则速度降低至 66%。
		/// </summary>
		public static void Tongue_ShortenRope(On.Player.Tongue.orig_decreaseRopeLength orig, Player.Tongue tongue, float amount)
		{
			if (tongue.player.IsCloud())
			{
				if (tongue.player.gourmandExhausted)
				{
					amount *= 0f;       // 完全阻止缩短
				}
				else
				{
					amount *= 0.66f;    // 减慢缩短速度
				}
			}
			orig(tongue, amount);
		}

		/// <summary>
		/// 检查当前抓握的物品是否可以被制作。Cloudtail 在上方向输入时，根据制作结果决定：
		/// 若结果可食用但胃已满则返回 false，否则允许制作非空结果。
		/// </summary>
		public static bool GraspsCanBeCrafted(On.Player.orig_GraspsCanBeCrafted orig, Player self)
		{
			if (self.IsCloud() && self.input[0].y == 1)
			{
				// 制作结果
				if (self.CraftingResults() == AbstractPhysicalObject.AbstractObjectType.DangleFruit)
				{
					if (self.FoodInStomach >= self.MaxFoodInStomach)
					{
						return false;
					}
					else return true;
				}
				else return self.CraftingResults() != null;
			}
			else return orig(self);
		}

		/// <summary>
		/// 被咬致死时的伤害倍率。Cloudtail 具有极高的抗咬能力（倍率 0.15）。
		/// </summary>
		public static float DeathByBiteMultiplier(On.Player.orig_DeathByBiteMultiplier orig, Player self)
		{
			if (self.IsCloud())
			{
				return 0.15f;
			}
			else return orig(self);
		}

		/// <summary>
		/// 判断是否满足猛击（SlugSlam）条件。Cloudtail 有大量额外限制，如不能对蛞蝓猫 NPC 使用、特定动画状态禁止、速度阈值等。
		/// </summary>
		public static bool SlugSlamConditions(On.Player.orig_SlugSlamConditions orig, Player self, PhysicalObject slamming)
		{
			if (self.IsCloud())
			{
				// 禁止对 SlugNPC 使用猛击
				if ((slamming as Creature).abstractCreature.creatureTemplate.type == MoreSlugcatsEnums.CreatureTemplateType.SlugNPC)
				{
					return false;
				}
				// 各种阻止状态
				if (self.gourmandAttackNegateTime > 0) return false;
				if (self.gravity == 0f) return false;
				if (self.cantBeGrabbedCounter > 0) return false;
				if (self.forceSleepCounter > 0) return false;
				if (self.timeSinceInCorridorMode < 5) return false;
				if (self.submerged) return false;
				if (self.enteringShortCut != null || (self.animation != Player.AnimationIndex.BellySlide && self.canJump >= 5))
					return false;
				// 禁止在特定动画下猛击
				if (self.animation == Player.AnimationIndex.CorridorTurn || self.animation == Player.AnimationIndex.CrawlTurn ||
					self.animation == Player.AnimationIndex.ZeroGSwim || self.animation == Player.AnimationIndex.ZeroGPoleGrab ||
					self.animation == Player.AnimationIndex.GetUpOnBeam || self.animation == Player.AnimationIndex.ClimbOnBeam ||
					self.animation == Player.AnimationIndex.AntlerClimb || self.animation == Player.AnimationIndex.BeamTip)
					return false;

				Vector2 vel = self.bodyChunks[0].vel;
				if (self.bodyChunks[1].vel.magnitude < vel.magnitude)
					vel = self.bodyChunks[1].vel;
				// 非腹滑状态下，垂直速度或总速度不足时不允许猛击
				if (self.animation != Player.AnimationIndex.BellySlide && vel.y >= -10f && vel.magnitude <= 25f)
					return false;

				Creature creature = slamming as Creature;
				foreach (Creature.Grasp grasp in self.grabbedBy)
				{
					if (grasp.pacifying || grasp.grabber == creature)
						return false;
				}
				// 合作模式中默认不允许对玩家猛击，除非开启友军伤害
				return !ModManager.CoopAvailable || !(slamming is Player) || Custom.rainWorld.options.friendlyFire;
			}
			else return orig(self, slamming);
		}

		/// <summary>
		/// 每帧更新 Cloudtail 的特殊状态：快速制作、疲劳系统（有氧水平）、存储导致的浮力/重力变化、滚动计时、体温源等。
		/// </summary>
		public static void MSCUpdate(On.Player.orig_UpdateMSC orig, Player self)
		{
			orig(self);
			if (self.TryGetCloud(out var data))
			{
				// 快速制作：若正在制作且反刍计数器小于 60，则直接设为 60 加快过程
				if (self.craftingObject && self.swallowAndRegurgitateCounter < 60)
				{
					self.swallowAndRegurgitateCounter = 60;
				}

				double exhaustion = 0.95; float recovery = 0.4f;
				if (data.Stuffed)
				{
					exhaustion = 0.75;    // 过饱时更容易疲劳
					recovery = 0.3f;
				}

				// 有氧水平达到阈值则进入疲劳状态
				if ((double)self.aerobicLevel >= exhaustion)
					self.gourmandExhausted = true;
				if (self.aerobicLevel < recovery)
					self.gourmandExhausted = false;

				if (self.gourmandExhausted)
				{
					self.slowMovementStun = Math.Max(self.slowMovementStun, (int)Custom.LerpMap(self.aerobicLevel, 0.7f, 0.4f, 6f, 0f));
					self.lungsExhausted = true;
				}
				data.Update();

				// 过饱增加浮力（下沉更慢）
				if (data.Stuffed)
					self.buoyancy = 0.8f;
				// 漂浮状态自定义重力
				if (data.Floaty)
					self.customPlayerGravity = 0.45f;

				// 滚动时延长滚动持续时间
				if (self.animation == Player.AnimationIndex.Roll)
				{
					Plugin.RollLength.TryGet(self, out int roll);
					data.Rolltimer += 1;
					if (data.Rolltimer < roll)
					{
						if (self.input[0].y < 0)
							self.rollCounter = 14;
					}
				}
				else
				{
					data.Rolltimer = 0;
				}

				// 根据环境热源缓慢降低低温值
				self.Hypothermia -= Mathf.Lerp(data.HeatSources(), 0f, self.HypothermiaExposure);
			}
		}

		/// <summary>
		/// 更新蛞蝓猫手部（SlugcatHand）的位置动画。当 Cloudtail 正在吞咽/反刍物品且手部对应空手槽时，调整手的视觉位置和抖动效果。
		/// </summary>
		public static void SlugcatHand_Update(On.SlugcatHand.orig_Update orig, SlugcatHand self)
		{
			orig(self);
			if ((self.owner.owner as Player).TryGetCloud(out var data))
			{
				if (data.SwallowOrRegurgitateCoutner > 10)
				{
					int num3 = -1;
					int num4 = 0;
					// 寻找一个空闲且可存储的抓握槽位
					while (num3 < 0 && num4 < 2)
					{
						if ((self.owner.owner as Player).grasps[num4] != null && data.CanStore())
						{
							num3 = num4;
						}
						num4++;
					}
					// 如果当前手部正是找到的槽位，则根据动画进度调整手部位置和抖动
					if (num3 == self.limbNumber)
					{
						float num5 = Mathf.InverseLerp(10f, 90f, (float)data.SwallowOrRegurgitateCoutner);
						if (num5 < 0.5f)
						{
							self.relativeHuntPos *= Mathf.Lerp(0.9f, 0.7f, num5 * 2f);
							self.relativeHuntPos.y += Mathf.Lerp(2f, 4f, num5 * 2f);
							self.relativeHuntPos.x *= Mathf.Lerp(1f, 1.2f, num5 * 2f);
						}
						else
						{
							(self.owner as PlayerGraphics).blink = 5;
							self.relativeHuntPos = new Vector2(0f, -4f) + Custom.RNV() * 2f * Random.value * Mathf.InverseLerp(0.5f, 1f, num5);
							(self.owner as PlayerGraphics).head.vel += Custom.RNV() * 2f * Random.value * Mathf.InverseLerp(0.5f, 1f, num5);
							self.owner.owner.bodyChunks[0].vel += Custom.RNV() * 0.2f * Random.value * Mathf.InverseLerp(0.5f, 1f, num5);
						}
					}
				}
			}
		}

		/// <summary>
		/// 圣徒（或 Cloudtail）的职业机制：按下跳跃键且满足条件时射出舌头。Cloudtail 也会执行此逻辑。
		/// </summary>
		public static void ClassMechanicsSaint(On.Player.orig_ClassMechanicsSaint orig, Player player)
		{
			orig(player);
			if (player.SlugCatClass.value != "Cloudtail" && player.SlugCatClass != MoreSlugcatsEnums.SlugcatStatsName.Saint)
			{
				return;
			}
			if (!MMF.cfgOldTongue.Value && player.input[0].jmp && !player.input[1].jmp && !player.input[0].pckp &&
				player.canJump <= 0 && player.bodyMode != Player.BodyModeIndex.Crawl &&
				player.animation != Player.AnimationIndex.ClimbOnBeam && player.animation != Player.AnimationIndex.AntlerClimb &&
				player.animation != Player.AnimationIndex.HangFromBeam && player.SaintTongueCheck())
			{
				Vector2 vector = new Vector2(player.flipDirection, 0.7f);
				Vector2 normalized = vector.normalized;
				if (player.input[0].y > 0)
				{
					normalized = new Vector2(0f, 1f);
				}
				normalized = (normalized + player.mainBodyChunk.vel.normalized * 0.2f).normalized;
				player.tongue.Shoot(normalized);
				player.AerobicIncrease(0.35f); // 增加有氧值（可能会移除？注释保留）
			}
		}

		/// <summary>
		/// 检查 Cloudtail 是否可以使用舌头。条件类似圣徒，但略有不同（如忽略走廊攀爬模式）。
		/// </summary>
		public static bool SaintTongueCheck(On.Player.orig_SaintTongueCheck orig, Player player)
		{
			if (player.IsCloud())
			{
				return player.Consious && player.tongue.mode == Player.Tongue.Mode.Retracted &&
					   player.bodyMode != Player.BodyModeIndex.CorridorClimb && !player.corridorDrop &&
					   player.bodyMode != Player.BodyModeIndex.ClimbIntoShortCut && player.bodyMode != Player.BodyModeIndex.WallClimb &&
					   player.bodyMode != Player.BodyModeIndex.Swimming &&
					   player.animation != Player.AnimationIndex.VineGrab && player.animation != Player.AnimationIndex.ZeroGPoleGrab;
			}
			else return orig(player);
		}

		/// <summary>
		/// 投掷物品时触发。Cloudtail 在疲劳或手持矛/莉莉浮标（非探险模式）时会改为轻轻放下并增加疲劳；
		/// 若过饱则直接让有氧水平达到最大；探险模式下允许投掷矛并重置疲劳。
		/// </summary>
		public static void ThrowObject(On.Player.orig_ThrowObject orig, Player self, int grasp, bool eu)
		{
			if (self.TryGetCloud(out var data))
			{
				string obj = self.grasps[grasp].grabbed.abstractPhysicalObject.type.value;
				if (self.gourmandExhausted)
				{
					Debug.Log("Forager exhausted, couldn't throw " + obj);
					self.TossObject(grasp, eu);
					self.AerobicIncrease(0.35f);
					self.ReleaseGrasp(grasp);
					return;
				}
				else if ((self.grasps[grasp].grabbed is Spear && (!ModManager.Expedition || (ModManager.Expedition && !self.room.game.rainWorld.ExpeditionMode))) ||
						 self.grasps[grasp].grabbed is LillyPuck)
				{
					Debug.Log("Forager in story mode, couldn't throw " + obj);
					self.TossObject(grasp, eu);
					self.AerobicIncrease(0.35f);
					self.ReleaseGrasp(grasp);
					return;
				}
				else if (self.grasps[grasp].grabbed is Spear && (ModManager.Expedition && self.room.game.rainWorld.ExpeditionMode))
				{
					Debug.Log("Forager threw Spear");
					self.aerobicLevel = 1f;   // 探险模式允许投矛但消耗全部耐力
				}
				if (data.Stuffed)
				{
					Debug.Log("Forager exhausted by full storage");
					self.aerobicLevel = 1f;
				}
			}
			orig(self, grasp, eu);
		}

		/// <summary>
		/// 投掷矛之后调整属性。Cloudtail 投出的矛拥有更高的伤害（1.8倍）和速度（1.25倍）。
		/// </summary>
		public static void ThrownSpear(On.Player.orig_ThrownSpear orig, Player self, Spear s)
		{
			orig(self, s);
			if (self.IsCloud())
			{
				s.spearDamageBonus = 1.8f;
				s.firstChunk.vel *= 1.25f;
			}
		}

		/// <summary>
		/// 初始化 PlayerGraphics 时，为 Cloudtail 设置特殊的尾巴段和舌头绳索段。
		/// </summary>
		public static void CosmeticsCtor(On.PlayerGraphics.orig_ctor orig, PlayerGraphics self, PhysicalObject ow)
		{
			orig(self, ow);
			if (self.player.IsCloud())
			{
				// 根据是否为幼崽设置不同尺寸的尾巴
				if (self.RenderAsPup)
				{
					self.tail[0] = new TailSegment(self, 8f, 2f, null, 0.85f, 1f, 1f, true);
					self.tail[1] = new TailSegment(self, 6f, 3.5f, self.tail[0], 0.85f, 1f, 0.5f, true);
					self.tail[2] = new TailSegment(self, 4f, 3.5f, self.tail[1], 0.85f, 1f, 0.5f, true);
					self.tail[3] = new TailSegment(self, 2f, 3.5f, self.tail[2], 0.85f, 1f, 0.5f, true);
				}
				else
				{
					self.tail[0] = new TailSegment(self, 12.5f, 4f, null, 0.85f, 1f, 1f, true);
					self.tail[1] = new TailSegment(self, 9.3f, 7f, self.tail[0], 0.85f, 1f, 0.5f, true);
					self.tail[2] = new TailSegment(self, 6.1f, 7f, self.tail[1], 0.85f, 1f, 0.5f, true);
					self.tail[3] = new TailSegment(self, 3f, 7f, self.tail[2], 0.85f, 1f, 0.5f, true);
				}

				// 将默认的尾巴段替换为自定义的尾巴
				var bp = self.bodyParts.ToList();
				bp.RemoveAll(x => x is TailSegment);
				bp.AddRange(self.tail);
				self.bodyParts = bp.ToArray();

				// 初始化绳索段（用于舌头可视化）和尾斑
				self.ropeSegments = new PlayerGraphics.RopeSegment[20]; // 数量可调整
				for (int k = 0; k < self.ropeSegments.Length; k++)
				{
					self.ropeSegments[k] = new PlayerGraphics.RopeSegment(k, self);
				}
				self.tailSpecks = new PlayerGraphics.TailSpeckles(self, 12);
			}
		}

		/// <summary>
		/// 存储每个 PlayerGraphics 的舌头精灵在精灵数组中的索引。
		/// </summary>
		public static Dictionary<PlayerGraphics, int> TongueSpriteIndex = new();

		/// <summary>
		/// 初始化精灵时，为 Cloudtail 添加舌头图形，并放到 Midground 容器中。
		/// </summary>
		public static void InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
		{
			orig(self, sLeaser, rCam);
			if (self.player.slugcatStats.name.value != "Cloudtail")// && self.player.SlugCatClass != MoreSlugcatsEnums.SlugcatStatsName.Saint)
			{
				return;
			}
			Array.Resize(ref sLeaser.sprites, sLeaser.sprites.Length + 1);

			// 记录舌头精灵索引
			if (TongueSpriteIndex.ContainsKey(self)) TongueSpriteIndex[self] = sLeaser.sprites.Length - 1;
			else TongueSpriteIndex.Add(self, sLeaser.sprites.Length - 1);

			// 创建舌头网格
			sLeaser.sprites[TongueSpriteIndex[self]] = TriangleMesh.MakeLongMesh(self.ropeSegments.Length - 1, false, true);

			// 将舌头精灵从默认容器移除，添加到 Midground
			sLeaser.sprites[TongueSpriteIndex[self]].RemoveFromContainer();
			rCam.ReturnFContainer("Midground").AddChild(sLeaser.sprites[TongueSpriteIndex[self]]);
		}

		/// <summary>
		/// 绘制 Cloudtail 的精灵：根据饱腹/漂浮状态调整身体缩放，处理反刍动画，绘制舌头。
		/// </summary>
		public static void DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
		{
			orig(self, sLeaser, rCam, timeStacker, camPos);
			if (self.player.TryGetCloud(out var data))
			{
				Plugin.PupWide.TryGet(self.player, out var pup_stomach_wide);
				Plugin.PupHigh.TryGet(self.player, out var pup_stomach_high);

				// 反刍动画更新
				if (data.SwallowOrRegurgitateCoutner > 0 && data.CanRetrieve())
				{
					data.UpdateRegurgitationGraphics(self, sLeaser);
				}

				float num = 0.5f + 0.5f * Mathf.Sin(Mathf.Lerp(self.lastBreath, self.breath, timeStacker) * Mathf.PI * 2f);
				// 替换头部元素为 Cloudtail 专属贴图
				if (!self.RenderAsPup)
				{
					sLeaser.sprites[3].element = Futile.atlasManager.GetElementWithName("Cloudtail" + sLeaser.sprites[3].element.name);
				}
				float Bodythickness = 1.5f;
				float HipsThickness = 1.75f;
				if (!self.RenderAsPup)
				{
					if (data.Stuffed)
					{
						Bodythickness = 1.65f;
						HipsThickness = 1.95f;
					}
					// 身体和臀部的缩放，受饱腹、蜷缩、呼吸和营养不良影响
					sLeaser.sprites[0].scaleX = Bodythickness + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;
					sLeaser.sprites[1].scaleY = 1.25f + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;
					sLeaser.sprites[1].scaleX = HipsThickness + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;
				}
				else
				{
					// 幼崽状态的缩放
					sLeaser.sprites[0].scaleX = pup_stomach_wide + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;
					sLeaser.sprites[0].scaleY = pup_stomach_high + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;
					sLeaser.sprites[1].scaleX = pup_stomach_wide + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;
					sLeaser.sprites[1].scaleY = pup_stomach_high + self.player.sleepCurlUp * 0.2f + 0.05f * num - 0.05f * self.malnourished;
				}
			}
			if (self.player.slugcatStats.name.value != "Cloudtail")// && self.player.SlugCatClass != MoreSlugcatsEnums.SlugcatStatsName.Saint)
			{
				return;
			}
			// 绘制舌头绳索（基于绳索段位置）
			Vector2 vector = Vector2.Lerp(self.drawPositions[0, 1], self.drawPositions[0, 0], timeStacker);
			Vector2 vector2 = Vector2.Lerp(self.drawPositions[1, 1], self.drawPositions[1, 0], timeStacker);
			float b = Mathf.Lerp(self.lastStretch, self.stretch, timeStacker);
			vector = Vector2.Lerp(self.ropeSegments[0].lastPos, self.ropeSegments[0].pos, timeStacker);
			vector += Custom.DirVec(Vector2.Lerp(self.ropeSegments[1].lastPos, self.ropeSegments[1].pos, timeStacker), vector) * 1f;
			float num7 = 0f;
			for (int k = 1; k < self.ropeSegments.Length; k++)
			{
				float num8 = (float)k / (float)(self.ropeSegments.Length - 1);
				if (k >= self.ropeSegments.Length - 2)
				{
					vector2 = new Vector2(sLeaser.sprites[9].x + camPos.x, sLeaser.sprites[9].y + camPos.y);
				}
				else
				{
					vector2 = Vector2.Lerp(self.ropeSegments[k].lastPos, self.ropeSegments[k].pos, timeStacker);
				}
				Vector2 a2 = Custom.PerpendicularVector((vector - vector2).normalized);
				float d4 = 0.2f + 1.6f * Mathf.Lerp(1f, b, Mathf.Pow(Mathf.Sin(num8 * Mathf.PI), 0.7f));
				Vector2 vector11 = vector - a2 * d4;
				Vector2 vector12 = vector2 + a2 * d4;
				float num9 = Mathf.Sqrt(Mathf.Pow(vector11.x - vector12.x, 2f) + Mathf.Pow(vector11.y - vector12.y, 2f));
				if (!float.IsNaN(num9))
				{
					num7 += num9;
				}
				(sLeaser.sprites[TongueSpriteIndex[self]] as TriangleMesh).MoveVertice((k - 1) * 4, vector11 - camPos);
				(sLeaser.sprites[TongueSpriteIndex[self]] as TriangleMesh).MoveVertice((k - 1) * 4 + 1, vector + a2 * d4 - camPos);
				(sLeaser.sprites[TongueSpriteIndex[self]] as TriangleMesh).MoveVertice((k - 1) * 4 + 2, vector2 - a2 * d4 - camPos);
				(sLeaser.sprites[TongueSpriteIndex[self]] as TriangleMesh).MoveVertice((k - 1) * 4 + 3, vector12 - camPos);
				vector = vector2;
			}
			// 根据舌头状态显示/隐藏舌头精灵
			if (self.player.tongue.Free || self.player.tongue.Attached)
			{
				sLeaser.sprites[TongueSpriteIndex[self]].isVisible = true;
			}
			else
			{
				sLeaser.sprites[TongueSpriteIndex[self]].isVisible = false;
			}
		}

		/// <summary>
		/// 更新 Cloudtail 尾巴物理：饱腹时尾巴下垂，漂浮时尾巴上浮。
		/// </summary>
		public static void UpdateGraphics(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
		{
			orig(self);

			if (self.player.TryGetCloud(out var data))
			{
				if (data.Stuffed)
				{
					self.tail[self.tail.Length - 1].vel.y -= 0.9f;
					self.tail[self.tail.Length - 1].vel.x *= 1.1f;
				}
				else if (data.Floaty)
				{
					self.tail[self.tail.Length - 1].vel.y += 0.9f;
					self.tail[self.tail.Length - 1].vel.x *= 1.1f;
				}
			}
		}

		/// <summary>
		/// 应用调色板时，为 Cloudtail 的舌头设置颜色（使用 SlugBase 自定义颜色索引 2）。
		/// </summary>
		public static void ApplyPalette(On.PlayerGraphics.orig_ApplyPalette orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
		{
			orig.Invoke(self, sLeaser, rCam, palette);
			if (self.player.slugcatStats.name.value != "Cloudtail")// && self.player.SlugCatClass != MoreSlugcatsEnums.SlugcatStatsName.Saint)
			{
				return;
			}
			// 注释掉了默认的基于雾色的舌头颜色，改为使用自定义颜色
			*//*
			float a = 0.95f;
			float b = 1f;
			float sl = 1f;
			float a2 = 0.75f;
			float b2 = 0.9f;
			for (int j = 0; j < (sLeaser.sprites[TongueSpriteIndex[self]] as TriangleMesh).verticeColors.Length; j++)
			{
				float num2 = Mathf.Clamp(Mathf.Sin((float)j / (float)((sLeaser.sprites[TongueSpriteIndex[self]] as TriangleMesh).verticeColors.Length - 1) * 3.1415927f), 0f, 1f);
				(sLeaser.sprites[TongueSpriteIndex[self]] as TriangleMesh).verticeColors[j] = Color.Lerp(palette.fogColor, Custom.HSL2RGB(Mathf.Lerp(a, b, num2), sl, Mathf.Lerp(a2, b2, Mathf.Pow(num2, 0.15f))), 0.7f);
			}
			*//*
			sLeaser.sprites[TongueSpriteIndex[self]].color = SlugBase.DataTypes.PlayerColor.GetCustomColor(self, 2);
		}

		/// <summary>
		/// 更新绳索段位置，用于 Cloudtail 的舌头可视化。复制自圣徒的绳索更新逻辑。
		/// </summary>
		public static void MSCUpdate(On.PlayerGraphics.orig_MSCUpdate orig, PlayerGraphics self)
		{
			orig.Invoke(self);
			if (self.player.slugcatStats.name.value != "Cloudtail")// && self.player.SlugCatClass != MoreSlugcatsEnums.SlugcatStatsName.Saint)
			{
				return;
			}
			// 绳索更新逻辑（与圣徒类似）
			self.lastStretch = self.stretch;
			self.stretch = self.RopeStretchFac;
			List<Vector2> list = new List<Vector2>();
			for (int j = self.player.tongue.rope.TotalPositions - 1; j > 0; j--)
			{
				list.Add(self.player.tongue.rope.GetPosition(j));
			}
			list.Add(self.player.mainBodyChunk.pos);
			float num = 0f;
			for (int k = 1; k < list.Count; k++)
			{
				num += Vector2.Distance(list[k - 1], list[k]);
			}
			float num2 = 0f;
			for (int l = 0; l < list.Count; l++)
			{
				if (l > 0)
				{
					num2 += Vector2.Distance(list[l - 1], list[l]);
				}
				self.AlignRope(num2 / num, list[l]);
			}
			for (int m = 0; m < self.ropeSegments.Length; m++)
			{
				self.ropeSegments[m].Update();
			}
			for (int n = 1; n < self.ropeSegments.Length; n++)
			{
				self.ConnectRopeSegments(n, n - 1);
			}
			for (int num3 = 0; num3 < self.ropeSegments.Length; num3++)
			{
				self.ropeSegments[num3].claimedForBend = false;
			}
		}

		/// <summary>
		/// 根据胃中存储的物品返回胃部发光颜色。灯笼为橙红色，神经元为绿色，监视者神经元为白色。
		/// </summary>
		public static Color? StomachGlowLightColor(On.Player.orig_StomachGlowLightColor orig, Player self)
		{
			if (self.TryGetCloud(out var data))
			{
				if (data.Storage.Length > 0)
				{
					if (data.StorageContains(AbstractPhysicalObject.AbstractObjectType.Lantern, false, null))
					{
						return new Color?(new Color(1f, 0.4f, 0.3f, 0.85f)); // 灯笼暖光
					}
					if (data.StorageContains(AbstractPhysicalObject.AbstractObjectType.NSHSwarmer, false, null) ||
						data.StorageContains(MoreSlugcatsEnums.AbstractObjectType.GlowWeed, false, null))
					{
						return new Color?(new Color(0.2f, 1f, 0.3f, 0.45f)); // 绿色发光（神经元/发光果）
					}
					if (data.StorageContains(AbstractPhysicalObject.AbstractObjectType.SSOracleSwarmer, false, null))
					{
						return new Color?(new Color(1f, 1f, 1f, 0.35f)); // 监视者神经元白光
					}
				}
				return null; // 无发光物品
			}
			return orig(self);
		}
	}
	*/
    #endregion

    #region 胖世界 BPOptions
    /*public class BPOptions : OptionInterface
	{


		public BPOptions()
		{
			//BPOptions.hardMode = this.config.Bind<bool>("hardMode", true, new ConfigurableInfo("(This is just info I guess)", null, "", new object[] { "something idk" }));

			BPOptions.hardMode = this.config.Bind<bool>("hardMode", false);
			BPOptions.holdShelterDoor = this.config.Bind<bool>("holdShelterDoor", false);
			BPOptions.backFoodStorage = this.config.Bind<bool>("backFoodStorage", false);
			BPOptions.easilyWinded = this.config.Bind<bool>("easilyWinded", false);
			BPOptions.extraTime = this.config.Bind<bool>("extraTime", true);
			BPOptions.hudHints = this.config.Bind<bool>("hudHints", true);
			BPOptions.fatArmor = this.config.Bind<bool>("fatArmor", true);
			BPOptions.slugSlams = this.config.Bind<bool>("slugSlams", true);
			//BPOptions.dietNeedles = this.config.Bind<bool>("dietNeedles", false);
			BPOptions.detachNeedles = this.config.Bind<bool>("detachNeedles", false);
			BPOptions.detachablePopcorn = this.config.Bind<bool>("detachablePopcorn", true);
			BPOptions.foodLoverPerk = this.config.Bind<bool>("foodLoverPerk", false);
			BPOptions.visualsOnly = this.config.Bind<bool>("visualsOnly", false);

			BPOptions.debugTools = this.config.Bind<bool>("debugTools", false);
			BPOptions.debugLogs = this.config.Bind<bool>("debugLogs", false);
			BPOptions.blushEnabled = this.config.Bind<bool>("blushEnabled", false);
			BPOptions.bpDifficulty = this.config.Bind<float>("bpDifficulty", -2f, new ConfigAcceptableRange<float>(-5f, 5f));
			BPOptions.sfxVol = this.config.Bind<float>("sfxVol", 0.1f, new ConfigAcceptableRange<float>(-0.1f, 0.4f));
			BPOptions.startThresh = this.config.Bind<int>("startThresh", 4, new ConfigAcceptableRange<int>(-4, 8));//(0, 4)
			BPOptions.gapVariance = this.config.Bind<float>("gapVariance", 1.0f, new ConfigAcceptableRange<float>(0.5f, 1.75f));
			BPOptions.jokeContent1 = this.config.Bind<bool>("jokeContent1", true);
			BPOptions.foodMult = this.config.Bind<int>("foodMult", 1, new ConfigAcceptableRange<int>(1, 4));
			BPOptions.meadowFoodStart = this.config.Bind<int>("meadowFoodStart", 4, new ConfigAcceptableRange<int>(0, 25));
			BPOptions.visualFatScale = this.config.Bind<float>("visualFatScale", 1.0f, new ConfigAcceptableRange<float>(0.33f, 1.00f));

			BPOptions.fatP1 = this.config.Bind<bool>("fatP1", true);
			BPOptions.fatP2 = this.config.Bind<bool>("fatP2", true);
			BPOptions.fatP3 = this.config.Bind<bool>("fatP3", true);
			BPOptions.fatP4 = this.config.Bind<bool>("fatP4", true);
			BPOptions.fatLiz = this.config.Bind<bool>("fatLiz", true);
			BPOptions.fatMice = this.config.Bind<bool>("fatMice", true);
			BPOptions.fatScavs = this.config.Bind<bool>("fatScavs", true);
			BPOptions.fatSquids = this.config.Bind<bool>("fatSquids", true);
			BPOptions.fatNoots = this.config.Bind<bool>("fatNoots", true);
			BPOptions.fatCentis = this.config.Bind<bool>("fatCentis", true);
			BPOptions.fatDll = this.config.Bind<bool>("fatDll", true);
			BPOptions.fatVults = this.config.Bind<bool>("fatVults", true);
			BPOptions.fatMiros = this.config.Bind<bool>("fatMiros", true);
			BPOptions.fatWigs = this.config.Bind<bool>("fatWigs", true);
			BPOptions.fatEels = this.config.Bind<bool>("fatEels", true);
			BPOptions.fatPups = this.config.Bind<bool>("fatPups", true);

			BPOptions.fatJets = this.config.Bind<bool>("fatJets", true);
			BPOptions.fatDeer = this.config.Bind<bool>("fatDeer", true);
			BPOptions.fatYeeks = this.config.Bind<bool>("fatYeeks", true);
			BPOptions.fatLeechs = this.config.Bind<bool>("fatLeechs", true);
			BPOptions.fatMoths = this.config.Bind<bool>("fatMoths", true);
			BPOptions.fatTards = this.config.Bind<bool>("fatTards", true);
			BPOptions.fatLoachs = this.config.Bind<bool>("fatLoachs", true);
		}



		public static Configurable<bool> blushEnabled;
		public static Configurable<bool> debugTools;
		public static Configurable<bool> holdShelterDoor;
		public static Configurable<bool> backFoodStorage;
		public static Configurable<bool> hardMode;
		public static Configurable<bool> easilyWinded;
		public static Configurable<bool> extraTime;
		public static Configurable<bool> hudHints;
		public static Configurable<bool> fatArmor;
		public static Configurable<bool> slugSlams;
		public static Configurable<bool> debugLogs;
		public static Configurable<float> bpDifficulty;
		public static Configurable<float> sfxVol;
		public static Configurable<int> startThresh;
		public static Configurable<float> gapVariance;
		public static Configurable<bool> detachNeedles;
		public static Configurable<bool> visualsOnly;
		public static Configurable<bool> jokeContent1;
		public static Configurable<bool> detachablePopcorn;
		public static Configurable<bool> foodLoverPerk;
		public static Configurable<int> foodMult;
		public static Configurable<int> meadowFoodStart;
		public static Configurable<float> visualFatScale;

		public static Configurable<bool> fatP1;
		public static Configurable<bool> fatP2;
		public static Configurable<bool> fatP3;
		public static Configurable<bool> fatP4;
		public static Configurable<bool> fatLiz;
		public static Configurable<bool> fatMice;
		public static Configurable<bool> fatScavs;
		public static Configurable<bool> fatSquids;
		public static Configurable<bool> fatNoots;
		public static Configurable<bool> fatCentis;
		public static Configurable<bool> fatDll;
		public static Configurable<bool> fatVults;
		public static Configurable<bool> fatMiros;
		public static Configurable<bool> fatWigs;
		public static Configurable<bool> fatEels;
		public static Configurable<bool> fatPups;
		public static Configurable<bool> fatJets;
		public static Configurable<bool> fatDeer;
		public static Configurable<bool> fatYeeks;
		public static Configurable<bool> fatLeechs;
		public static Configurable<bool> fatMoths;
		public static Configurable<bool> fatTards;
		public static Configurable<bool> fatLoachs;
		//Lizard
		//Lantern Mice
		//Scavengers

		//Squidcada
		//Noodle Flies
		//Centipedes

		//DLL
		//Vultures
		//Miros Birds

		//Dropwigs
		//Leviathan


		private OpSimpleButton presetSilly;
		private OpSimpleButton presetBalanced;
		private OpSimpleButton presetPipeCleaner;


		public override void Update()
		{
			base.Update();

			//this.Tabs[0].items

			if (this.chkBoxVisOnly != null)
			{
				if (this.chkBoxVisOnly.GetValueBool() == true)
				{
					this.diffSlide.greyedOut = true;
					this.opLab1.Hidden = true;
					this.opLab2.Hidden = true;
					for (int i = 1; i < myBoxes.Length; i++)
					{
						if (myBoxes[i] != null)
							myBoxes[i].greyedOut = true;
					}
				}
				else
				{
					this.diffSlide.greyedOut = false;
					this.opLab1.Hidden = false;
					this.opLab2.Hidden = false;
					for (int i = 1; i < myBoxes.Length; i++)
					{
						if (myBoxes[i] != null)
							myBoxes[i].greyedOut = false;
					}
				}
			}

		}

		public static string BPTranslate(string t)
		{
			return OptionInterface.Translate(t); //this.manager.rainWorld.inGameTranslator.BPTranslate(t);
		}


		public OpFloatSlider diffSlide;
		public OpCheckBox chkBoxVisOnly;

		public OpCheckBox chkBoxslugSlams;
		public OpCheckBox chkBoxNeedles;

		public OpLabel opLab1;
		public OpLabel opLab2;

		public static OpCheckBox[] myBoxes;


		public void SillyPreset(UIfocusable trigger)
		{
			//for (int i = 0; i < MMF.boolPresets.Count; i++)
			//{
			//    if (MMF.boolPresets[i].config.BoundUIconfig != null)
			//    {
			//        //MMF.boolPresets[i].config.BoundUIconfig.value = ValueConverter.ConvertToString<bool>(MMF.boolPresets[i].remixValue);
			//    }
			//}
			this.diffSlide.SetValueFloat(-2f);
		}

		public void BalancedPreset(UIfocusable trigger)
		{
			this.diffSlide.SetValueFloat(0f);
		}

		public void PipeCleanerPreset(UIfocusable trigger)
		{
			this.diffSlide.SetValueFloat(3f);
		}


		public override void Initialize()
		{
			base.Initialize();

			// OpTab opTab = new OpTab(this, "Options");
			this.Tabs = new OpTab[]
			{
				//opTab
				new OpTab(this, BPTranslate("Options")),
				new OpTab(this, BPTranslate("Misc")),
				new OpTab(this, BPTranslate("Creatures")),
				new OpTab(this, BPTranslate("Info"))
			};

			Vector2 btnSize = new Vector2(130f, 25f);
			float btnHeight = 548f;
			Tabs[0].AddItems(
				 this.presetSilly = new OpSimpleButton(new Vector2(40f, btnHeight), btnSize, "Silly") { description = OptionInterface.Translate("A more relaxed and goofy experience with fewer downsides to becoming round (Default)") },
				 this.presetBalanced = new OpSimpleButton(new Vector2(40f + 145f, btnHeight), btnSize, "Balanced") { description = OptionInterface.Translate("The original mod experience of risk and reward where each extra pip comes with tradeoffs to consider") },
				 this.presetPipeCleaner = new OpSimpleButton(new Vector2(40f + 290f, btnHeight), btnSize, "Pipe Cleaner") { description = OptionInterface.Translate("A challenge mode for only the most stubborn food enthusiests") }
			   );
			Tabs[0].AddItems(new OpLabel(40F, 578, BPTranslate("Difficulty Presets")));
			this.presetSilly.OnClick += this.SillyPreset;
			this.presetBalanced.OnClick += this.BalancedPreset;
			this.presetPipeCleaner.OnClick += this.PipeCleanerPreset;


			float lineCount = 500;

			Tabs[0].AddItems(new OpLabel(175f, 595, BPTranslate("Hover over a setting to read more info about it")));


			//OpLabel opLabel = new OpLabel(new Vector2(100f, opRect.size.y - 25f), new Vector2(30f, 25f), ":(", FLabelAlignment.Left, true, null)
			//OpCheckBox opCheckBox = new OpCheckBox(this.config as Configurable<bool>, posX, posY)
			//this.numberPlayersSlider = new OpSliderTick(menu.oi.config.Bind<int>("_cosmetic", Custom.rainWorld.options.JollyPlayerCount, new ConfigAcceptableRange<int>(1, 4)), this.playerSelector[0].pos + new Vector2((float)num / 2f, 130f), (int)(this.playerSelector[3].pos - this.playerSelector[0].pos).x, false);
			// Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, "Makes squeezing through pipes even more difficult for fatter creatures") { description = "This is My Text" });

			//Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, "Makes squeezing through pipes even more difficult for fatter creatures") { description = "This is My Text" });
			// Tabs[0].AddItems(new OpSlider(BPOptions.bpDifficulty, new Vector2(50f, lineCount), 50, false));
			this.diffSlide = new OpFloatSlider(BPOptions.bpDifficulty, new Vector2(55f, lineCount - 0), 250, 0, false);
			string discDiff = BPTranslate("Sets the average difficulty for squeezing through pipes, and the impact weight has on your agility");
			Tabs[0].AddItems(this.diffSlide, new OpLabel(50f, lineCount - 15, BPTranslate("Pipe Size Difficulty")) { bumpBehav = this.diffSlide.bumpBehav, description = discDiff });
			this.diffSlide.description = discDiff;
			Tabs[0].AddItems(this.opLab1 = new OpLabel(15f, lineCount + 5, BPTranslate("Wide")) { description = BPTranslate("Easy") });
			Tabs[0].AddItems(this.opLab2 = new OpLabel(320f, lineCount + 5, BPTranslate("Snug")) { description = BPTranslate("Hard") });

			//OpCheckBox chkBox5 = new OpCheckBox(BPOptions.hardMode, new Vector2(15f, lineCount));
			//Tabs[0].AddItems(chkBox5, new OpLabel(45f, lineCount, "Snug Pipes") { bumpBehav = chkBox5.bumpBehav });



			*//*
			OpFloatSlider agilSlide = new OpFloatSlider(BPOptions.agilityDiff, new Vector2(55f, lineCount - 0), 250, 0, false);
			Tabs[0].AddItems(agilSlide, new OpLabel(50f, lineCount - 15, BPTranslate("Agility Penalty")) { bumpBehav = agilSlide.bumpBehav, description = BPTranslate("Your weight has a more noticeable impact on your ability to run, climb and jump") });

			Tabs[0].AddItems(new OpLabel(15f, lineCount + 5, BPTranslate("Easy")) );
			Tabs[0].AddItems(new OpLabel(320f, lineCount +5, BPTranslate("Hard")) );
			*//*


			string dscVisuals = BPTranslate("Removes all gameplay changes except visual ones");
			this.chkBoxVisOnly = new OpCheckBox(BPOptions.visualsOnly, new Vector2(15f + 425, lineCount));
			Tabs[0].AddItems(this.chkBoxVisOnly, new OpLabel(45f + 425, lineCount, BPTranslate("Visuals only")) { bumpBehav = this.chkBoxVisOnly.bumpBehav, description = dscVisuals });
			this.chkBoxVisOnly.description = dscVisuals;
			//chkBoxVisOnly.Hidden = true;



			float indenting = 250f;

			lineCount -= 70;
			OpSlider threshSlide = new OpSlider(BPOptions.startThresh, new Vector2(55f, lineCount - 0), 150, false);
			string dscThresh = BPTranslate("Offset how much food you need to eat to get fat. Lower values will make you fat earlier.");
			// Tabs[0].AddItems(threshSlide, new OpLabel(50f, lineCount - 15, BPTranslate("Starting threshold")) { bumpBehav = threshSlide.bumpBehav, description = BPTranslate("Sets how close to full you must be before eating food will add weight. Lower values will make you fat earlier.") });
			Tabs[0].AddItems(threshSlide, new OpLabel(50f, lineCount - 15, BPTranslate("Starting threshold")) { bumpBehav = threshSlide.bumpBehav, description = dscThresh });
			Tabs[0].AddItems(new OpLabel(15f, lineCount + 5, BPTranslate("Early")) { description = BPTranslate("You will start getting fat before your belly is full.") });
			Tabs[0].AddItems(new OpLabel(220f, lineCount + 5, BPTranslate("Late")) { description = BPTranslate("You won't start getting fat until your belly is full") });
			threshSlide.description = dscThresh;


			OpFloatSlider varianceSlide = new OpFloatSlider(BPOptions.gapVariance, new Vector2(350f, lineCount - 0), 150, 1, false);
			dscThresh = BPTranslate("Determines how wide the range of gap sizes can be. Wider variety makes easy gaps easier and harder gaps harder");
			Tabs[0].AddItems(varianceSlide, new OpLabel(varianceSlide.pos.x + 25f, lineCount - 15, BPTranslate("Pipe size variety")) { bumpBehav = varianceSlide.bumpBehav, description = dscThresh });
			Tabs[0].AddItems(new OpLabel(varianceSlide.pos.x - 45f, lineCount + 5, BPTranslate("Similar")) { description = BPTranslate("Gap sizes will be similar to each other") });
			Tabs[0].AddItems(new OpLabel(varianceSlide.pos.x + 160f, lineCount + 5, BPTranslate("Diverse")) { description = BPTranslate("Gap sizes will vary widely") });
			varianceSlide.description = dscThresh;





			//Pipes are less snug and easier to wiggle through, even when very fat
			//Makes squeezing through pipes even more difficult for fatter creatures
			//Snug Pipes
			lineCount -= 60;
			BPOptions.hardMode.Value = false;

			//OKAY MAKE IT BUT DON'T SHOW IT
			*//*-------------------------------------
			string dsc5 = BPTranslate("Outgrowing pipes is more punishing on how long it takes to wiggle through");
			//Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, BPTranslate("Outgrowing pipes is more punishing on how long it takes to wiggle through")) );
			OpCheckBox chkBox5 = new OpCheckBox(BPOptions.hardMode, new Vector2(15f + 800f, lineCount - 120f));
			Tabs[0].AddItems(chkBox5, new OpLabel(45f + 800f, lineCount - 120, BPTranslate("Unforgiving Gap Sizes")) { bumpBehav = chkBox5.bumpBehav, description = dsc5 });
			chkBox5.description = dsc5;
			*//*

			string dscHints = BPTranslate("Occasionally show in-game hints related to controls and mechanics of the mod");
			//Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, "Occasionally show in-game hints related to controls and mechanics of the mod") ); //{ description = "This is My Text" }
			OpCheckBox chkBoxHints = new OpCheckBox(BPOptions.hudHints, new Vector2(15f, lineCount));
			Tabs[0].AddItems(chkBoxHints, new OpLabel(45f, lineCount, BPTranslate("Hud Hints")) { bumpBehav = chkBoxHints.bumpBehav, description = dscHints });
			chkBoxHints.description = dscHints;


			string dscArmor = BPTranslate("Increase resistance to bites based on how fat you are") + ", as well as spears, stuns, and coldness";
			OpCheckBox chkBoxArmor = new OpCheckBox(BPOptions.fatArmor, new Vector2(15f + indenting, lineCount));
			Tabs[0].AddItems(chkBoxArmor, new OpLabel(45f + indenting, lineCount, BPTranslate("Fat Armor")) { bumpBehav = chkBoxArmor.bumpBehav, description = dscArmor });
			chkBoxArmor.description = dscArmor;





			lineCount -= 40;
			string dsc6 = BPTranslate("Your weight has a more noticeable impact on your ability to run, climb and jump");
			//Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, BPTranslate("Your weight has a more noticeable impact on your ability to run, climb and jump")) );
			OpCheckBox chkBox6 = new OpCheckBox(BPOptions.easilyWinded, new Vector2(15f, lineCount));
			Tabs[0].AddItems(chkBox6, new OpLabel(45f, lineCount, BPTranslate("Easily Winded")) { bumpBehav = chkBox6.bumpBehav, description = dsc6 });
			chkBox6.description = dsc6;


			string dscCorn = BPTranslate("Popcorn plants can be torn from their stems");
			OpCheckBox mpBox1 = new OpCheckBox(BPOptions.detachablePopcorn, new Vector2(15f + indenting, lineCount));
			Tabs[0].AddItems(mpBox1, new OpLabel(45f + indenting, lineCount, BPTranslate("Detachable Popcorn Plants")) { bumpBehav = mpBox1.bumpBehav, description = dscCorn });
			mpBox1.description = dscCorn;





			lineCount -= 40;
			string dsc4 = BPTranslate("Double-tap the Grab button to store an edible item on your back like a spear");
			//Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, BPTranslate("Double-tap the Grab button to store an edible item on your back like a spear")) );
			OpCheckBox chkBox4 = new OpCheckBox(BPOptions.backFoodStorage, new Vector2(15f, lineCount));
			Tabs[0].AddItems(chkBox4, new OpLabel(45f, lineCount, BPTranslate("Back Food Storage")) { bumpBehav = chkBox4.bumpBehav, description = dsc4 });
			chkBox4.description = dsc4;




			string dscFood = BPTranslate("Allows the player to eat all food types for their full value");
			OpCheckBox mpBox2 = new OpCheckBox(BPOptions.foodLoverPerk, new Vector2(15f + indenting, lineCount));
			Tabs[0].AddItems(mpBox2, new OpLabel(45f + indenting, lineCount, BPTranslate("Food Lover")) { bumpBehav = mpBox2.bumpBehav, description = dscFood });
			mpBox2.description = dscFood;


			lineCount -= 40;
			string dsc7 = BPTranslate("Slightly increase the cycle timer to account for the slowdowns");
			//Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, BPTranslate("Slightly increase the cycle timer to account for the slowdowns")) );
			OpCheckBox chkBox7 = new OpCheckBox(BPOptions.extraTime, new Vector2(15f, lineCount));
			Tabs[0].AddItems(chkBox7, new OpLabel(45f, lineCount, BPTranslate("Extra Cycle Time")) { bumpBehav = chkBox7.bumpBehav, description = dsc7 });
			chkBox7.description = dsc7;


			if (ModManager.MSC)
			{
				string dscSlams = BPTranslate("All slugcats can do Gourmand's body slam, if fat enough") + " (" + BPTranslate("an audio queue will play" + ")");
				this.chkBoxslugSlams = new OpCheckBox(BPOptions.slugSlams, new Vector2(15f + indenting, lineCount));
				Tabs[0].AddItems(this.chkBoxslugSlams, new OpLabel(45f + indenting, lineCount, BPTranslate("Slug Slams")) { bumpBehav = this.chkBoxslugSlams.bumpBehav, description = dscSlams });
				this.chkBoxslugSlams.description = dscSlams;
			}
			else
			{
				BPOptions.slugSlams.Value = false;
			}


			if (ModManager.MSC)
			{
				lineCount -= 40;
				//string dscNeedles = BPTranslate("Spearmaster's needles will gain less food when your belly is full");
				//Diet Needles
				string dscNeedles = BPTranslate("When Spearmaster is full, switching hands (double tap grab) will detatch your needles");
				this.chkBoxNeedles = new OpCheckBox(BPOptions.detachNeedles, new Vector2(15f + 0, lineCount));
				Tabs[0].AddItems(this.chkBoxNeedles, new OpLabel(45f + 0, lineCount, BPTranslate("Detachable Needles")) { bumpBehav = this.chkBoxNeedles.bumpBehav, description = dscNeedles });
				this.chkBoxNeedles.description = dscNeedles;
			}


			//lineCount -= 40;
			OpSlider meadowSizeSlider = new OpSlider(BPOptions.meadowFoodStart, new Vector2(15f + indenting, lineCount - 10), 200, false);
			string dscMeadowSizeSlider = BPTranslate("Set how rotund your character will be when joining Meadow-Mode lobbies online. Requires the Rain Meadow mod");
			Tabs[0].AddItems(meadowSizeSlider, new OpLabel(15f + indenting, lineCount - 25, BPTranslate("Meadow Mode character fatness")) { bumpBehav = meadowSizeSlider.bumpBehav, description = dscMeadowSizeSlider });
			meadowSizeSlider.description = dscMeadowSizeSlider;



			myBoxes = new OpCheckBox[10];
			myBoxes[0] = null; // chkBox5;
			myBoxes[1] = chkBoxArmor;
			myBoxes[2] = chkBox6;
			myBoxes[3] = chkBox4;
			myBoxes[4] = chkBox7;
			myBoxes[5] = chkBoxHints;
			myBoxes[6] = mpBox1;
			myBoxes[7] = mpBox2;
			if (ModManager.MSC)
			{
				myBoxes[8] = this.chkBoxslugSlams;
				myBoxes[9] = this.chkBoxNeedles;
			}



			//EHH, WE CAN DO WTHIS WITH SANDBOX MODE
			//OpCheckBox chkBox3 = new OpCheckBox(50f, 250f, "noRain", false);
			//      Tabs[0].AddItems(chkBox3,
			//          new OpLabel(50f, 280f, "No Rain") { bumpBehav = chkBox2.bumpBehav });

			//lineCount -= 65;
			//Tabs[0].AddItems(new OpLabel(50f, lineCount - 20f, "If enabled; shelter doors won't automatically close unless a player holds down to sleep") { description = "This is My Text" });
			//OpCheckBox chkBox3 = new OpCheckBox(BPOptions.holdShelterDoor, new Vector2(15f, lineCount));
			//Tabs[0].AddItems(chkBox3, new OpLabel(45f, lineCount, "Hold Shelter Doors") { bumpBehav = chkBox3.bumpBehav });







			//------------------ NEW TAB FOR OTHER STUFF



			lineCount = 550;
			Tabs[1].AddItems(new OpLabel(50f, lineCount - 20f, BPTranslate("Press Throw + Jump to add food pips. Press Crouch + Throw + Jump to subtract")));
			OpCheckBox chkBox2 = new OpCheckBox(BPOptions.debugTools, new Vector2(15f, lineCount));
			Tabs[1].AddItems(chkBox2, new OpLabel(45f, lineCount, BPTranslate("Debug Tools")) { bumpBehav = chkBox2.bumpBehav });

			lineCount -= 65;
			Tabs[1].AddItems(new OpLabel(50f, lineCount - 20f, BPTranslate("Development logs. For development things")));
			OpCheckBox chkLogs = new OpCheckBox(BPOptions.debugLogs, new Vector2(15f, lineCount));
			Tabs[1].AddItems(chkLogs, new OpLabel(45f, lineCount, BPTranslate("Debug Logs")) { bumpBehav = chkLogs.bumpBehav });


			lineCount -= 65;
			Tabs[1].AddItems(new OpLabel(50f, lineCount - 20f, BPTranslate("Adds a panting and red-faced visual effect when struggling for long periods of time")));
			OpCheckBox chkExample = new OpCheckBox(BPOptions.blushEnabled, new Vector2(15f, lineCount));
			Tabs[1].AddItems(chkExample, new OpLabel(45f, lineCount, BPTranslate("Exhaustion FX")) { bumpBehav = chkExample.bumpBehav });
			Tabs[1].AddItems(new OpLabel(50f, lineCount - 35f, BPTranslate("(can cause some visual glitches with held items)")));


			lineCount -= 95;
			OpFloatSlider sfxSlide = new OpFloatSlider(BPOptions.sfxVol, new Vector2(30f, lineCount - 25), 250, 2, false);
			string dscSFXvol = BPTranslate("Volume of the squeeze sound effect when slugcats are stuck");
			Tabs[1].AddItems(sfxSlide, new OpLabel(30f, lineCount + 15, BPTranslate("Squeeze SFX Volume")) { bumpBehav = sfxSlide.bumpBehav, description = dscSFXvol });
			Tabs[1].AddItems(new OpLabel(10f, lineCount - 40f, BPTranslate("(If the sfx is too soft or played without headphones)")));
			sfxSlide.description = dscSFXvol;

			OpSlider foodMultSlide = new OpSlider(BPOptions.foodMult, new Vector2(325f, lineCount - 25), 125, false);
			string dscFoodMult = BPTranslate("Multiplies how much food you get per-food");
			Tabs[1].AddItems(foodMultSlide, new OpLabel(325f, lineCount + 15, BPTranslate("Food Multiplier")) { bumpBehav = foodMultSlide.bumpBehav, description = dscFoodMult });
			foodMultSlide.description = dscFoodMult;

			OpFloatSlider fatScaleSlider = new OpFloatSlider(BPOptions.visualFatScale, new Vector2(475f, lineCount - 25), 125, 1, false); //WHY DOES THIS NOT WORK IF ITS 2 DECIMALS?????? HELLO???
			string dscFatScale = BPTranslate("The scale at which food points affect your visual size. Lower numbers makes fat look less pronounced.");
			Tabs[1].AddItems(fatScaleSlider, new OpLabel(475f, lineCount + 15, BPTranslate("Fat Visual Scale")) { bumpBehav = fatScaleSlider.bumpBehav, description = dscFatScale });
			fatScaleSlider.description = dscFatScale;

			lineCount -= 80;
			Tabs[1].AddItems(new OpLabel(50f, lineCount, BPTranslate("Tip: The squeeze sfx pitch hints how close you are to popping free")));
			lineCount -= 20;
			Tabs[1].AddItems(new OpLabel(50f, lineCount, BPTranslate("Spending lots of stamina when you are close to freedom can help you pop through early!")));


			for (int j = 0; j < 2; j++)
			{
				int descLine = 155;
				Tabs[j].AddItems(new OpLabel(25f, descLine + 25f, "--- MOD FEATURES ---"));
				// Tabs[0].AddItems(new OpLabel(25f, descLine, "Press up against stuck creatures to push them. Grab them to pull"));
				// descLine -= 20;
				Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("Press against stuck creatures to push them. Grab them and move backwards to pull")));
				descLine -= 20;
				Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("Press Jump while pushing or pulling to strain harder and spend stamina")));
				descLine -= 20;
				Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("Pivot dash, belly slide, or charge-jump into stuck creatures to ram them")));
				descLine -= 20;
				Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("Spending stamina too quickly can make you exhausted and slow down progress")));
				descLine -= 30;
				//descLine -= 20;
				// Tabs[0].AddItems(new OpLabel(25f, 140f, "Certain fruits can be used to slicken up stuck creatures. Push against them with fruit in hand to apply"));
				Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("Certain fruits can be used to slicken up stuck creatures. (blue fruit, slime mold, mushrooms, etc)")));
				descLine -= 20;
				Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("While stuck, tab the Grab button to smear fruit on yourself")));
				descLine -= 20;
				Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("Push against stuck creatures with fruit in hand and press Jump to smear it on them")));
				descLine -= 25;
				if (ModManager.JollyCoop)
					Tabs[j].AddItems(new OpLabel(25f, descLine, BPTranslate("Hold Grab + Throw while holding food next to a co-op partner to feed them")));
			}




			//SIIIIGH.... FIIIIINE....

			int critTab = 2;
			float xPad = 30f;
			float yPad = 3f;
			//Slugcat

			//Lizard
			//Lantern Mice
			//Scavengers

			//Squidcada
			//Noodle Flies
			//Centipedes

			//DLL
			//Vultures
			//Miros Birds

			//Dropwigs
			//Leviathan


			Tabs[critTab].AddItems(new OpLabel(125f, 575f, BPTranslate("Select which creatures can become fat"), bigText: true));

			lineCount = 515;
			int baseMargin = 65;
			int margin = baseMargin;
			string dsc = "";

			Tabs[critTab].AddItems(new UIelement[]
			{
				new OpRect(new Vector2(0, lineCount - 15), new Vector2(600, 55))
			});

			OpCheckBox pBox1;
			dsc = BPTranslate("Player") + " 1";
			Tabs[critTab].AddItems(new UIelement[]
			{
				pBox1 = new OpCheckBox(BPOptions.fatP1, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(pBox1.pos.x + xPad, pBox1.pos.y + yPad, dsc)
				{description = dsc}  //bumpBehav = chkBox5.bumpBehav, 
			});

			margin += 125;
			OpCheckBox pBox2;
			dsc = BPTranslate("Player") + " 2";
			Tabs[critTab].AddItems(new UIelement[]
			{
				pBox2 = new OpCheckBox(BPOptions.fatP2, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(pBox2.pos.x + xPad, pBox2.pos.y + yPad, dsc)
				{description = dsc}
			});

			margin += 125;
			OpCheckBox pBox3;
			dsc = BPTranslate("Player") + " 3";
			Tabs[critTab].AddItems(new UIelement[]
			{
				pBox3 = new OpCheckBox(BPOptions.fatP3, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(pBox3.pos.x + xPad, pBox3.pos.y + yPad, dsc)
				{description = dsc}
			});

			margin += 125;
			OpCheckBox pBox4;
			dsc = BPTranslate("Player") + " 4";
			Tabs[critTab].AddItems(new UIelement[]
			{
				pBox4 = new OpCheckBox(BPOptions.fatP4, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(pBox4.pos.x + xPad, pBox4.pos.y + yPad, dsc)
				{description = dsc}
			});



			//---------------- crits-------------
			margin = baseMargin;
			lineCount -= 75;
			float linePadding = 45f;

			Tabs[critTab].AddItems(new UIelement[]
			{
				new OpRect(new Vector2(0, lineCount - 285), new Vector2(600, 405))
			});

			OpCheckBox critBox1;
			dsc = BPTranslate("Lizard"); //creaturetype-GreenLizard
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox1 = new OpCheckBox(BPOptions.fatLiz, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox1.pos.x + xPad, critBox1.pos.y + yPad, dsc)
				{description = dsc}  //bumpBehav = chkBox5.bumpBehav, 
			});


			margin += 175;
			OpCheckBox critBox2;
			dsc = BPTranslate("creaturetype-LanternMouse");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox2 = new OpCheckBox(BPOptions.fatMice, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox2.pos.x + xPad, critBox2.pos.y + yPad, dsc)
				{description = dsc}
			});



			margin += 175;
			OpCheckBox critBox3;
			dsc = BPTranslate("creaturetype-Scavenger");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox3 = new OpCheckBox(BPOptions.fatScavs, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox3.pos.x + xPad, critBox3.pos.y + yPad, dsc)
				{description = dsc}
			});



			margin = baseMargin;
			lineCount -= linePadding;
			OpCheckBox critBox4;
			dsc = BPTranslate("creaturetype-CicadaA");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox4 = new OpCheckBox(BPOptions.fatSquids, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox4.pos.x + xPad, critBox4.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin += 175;
			OpCheckBox critBox5;
			dsc = BPTranslate("creaturetype-BigNeedleWorm");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox5 = new OpCheckBox(BPOptions.fatNoots, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox5.pos.x + xPad, critBox5.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin += 175;
			OpCheckBox critBox6;
			dsc = BPTranslate("creaturetype-Centipede");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox6 = new OpCheckBox(BPOptions.fatCentis, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox6.pos.x + xPad, critBox6.pos.y + yPad, dsc)
				{description = dsc}
			});

			//DLL
			//Vultures
			//Miros Birds
			margin = baseMargin;
			lineCount -= linePadding;
			OpCheckBox critBox7;
			dsc = BPTranslate("creaturetype-DaddyLongLegs");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox7 = new OpCheckBox(BPOptions.fatDll, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox7.pos.x + xPad, critBox7.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin += 175;
			OpCheckBox critBox8;
			dsc = BPTranslate("creaturetype-Vulture");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox8 = new OpCheckBox(BPOptions.fatVults, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox8.pos.x + xPad, critBox8.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin += 175;
			OpCheckBox critBox9;
			dsc = BPTranslate("creaturetype-MirosBird");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox9 = new OpCheckBox(BPOptions.fatMiros, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox9.pos.x + xPad, critBox9.pos.y + yPad, dsc)
				{description = dsc}
			});

			//Dropwigs
			//Leviathan
			margin = baseMargin;
			lineCount -= linePadding;
			OpCheckBox critBox10;
			dsc = BPTranslate("creaturetype-DropBug");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox10 = new OpCheckBox(BPOptions.fatWigs, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox10.pos.x + xPad, critBox10.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin += 175;
			OpCheckBox critBox11;
			dsc = BPTranslate("creaturetype-BigEel");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox11 = new OpCheckBox(BPOptions.fatEels, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox11.pos.x + xPad, critBox11.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin += 175;
			//OpCheckBox critBox11;
			dsc = BPTranslate("creaturetype-JetFish");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox11 = new OpCheckBox(BPOptions.fatJets, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox11.pos.x + xPad, critBox11.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin = baseMargin;
			lineCount -= linePadding;
			dsc = BPTranslate("creaturetype-Deer");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox11 = new OpCheckBox(BPOptions.fatDeer, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox11.pos.x + xPad, critBox11.pos.y + yPad, dsc)
				{description = dsc}
			});


			margin += 175;
			//OpCheckBox critBox11;
			dsc = BPTranslate("creaturetype-Leech");
			Tabs[critTab].AddItems(new UIelement[]
			{
				critBox11 = new OpCheckBox(BPOptions.fatLeechs, new Vector2(margin, lineCount))
				{description = dsc},
				new OpLabel(critBox11.pos.x + xPad, critBox11.pos.y + yPad, dsc)
				{description = dsc}
			});


			if (ModManager.MSC)
			{
				margin += 175;
				OpCheckBox critBox13;
				dsc = BPTranslate("creaturetype-Yeek");
				Tabs[critTab].AddItems(new UIelement[]
				{
					critBox13 = new OpCheckBox(BPOptions.fatYeeks, new Vector2(margin, lineCount))
					{description = dsc},
					new OpLabel(critBox13.pos.x + xPad, critBox13.pos.y + yPad, dsc)
					{description = dsc}
				});



				margin = baseMargin;
				lineCount -= linePadding;
				OpCheckBox critBox12;
				dsc = BPTranslate("creaturetype-SlugNPC");
				Tabs[critTab].AddItems(new UIelement[]
				{
					critBox12 = new OpCheckBox(BPOptions.fatPups, new Vector2(margin, lineCount))
					{description = dsc},
					new OpLabel(critBox12.pos.x + xPad, critBox12.pos.y + yPad, dsc)
					{description = dsc}
				});
			}
			else
			{
				BPOptions.fatPups.Value = true;
			}


			if (ModManager.Watcher)
			{
				margin = baseMargin;
				lineCount -= linePadding;

				OpCheckBox critBox13;
				dsc = BPTranslate("creaturetype-BigMoth");
				Tabs[critTab].AddItems(new UIelement[]
				{
					critBox13 = new OpCheckBox(BPOptions.fatMoths, new Vector2(margin, lineCount))
					{description = dsc},
					new OpLabel(critBox13.pos.x + xPad, critBox13.pos.y + yPad, dsc)
					{description = dsc}
				});


				margin += 175;
				OpCheckBox critBox12;
				dsc = BPTranslate("creaturetype-Tardigrade");
				Tabs[critTab].AddItems(new UIelement[]
				{
					critBox12 = new OpCheckBox(BPOptions.fatTards, new Vector2(margin, lineCount))
					{description = dsc},
					new OpLabel(critBox12.pos.x + xPad, critBox12.pos.y + yPad, dsc)
					{description = dsc}
				});

				margin += 175;
				dsc = BPTranslate("creaturetype-Loach");
				Tabs[critTab].AddItems(new UIelement[]
				{
					critBox12 = new OpCheckBox(BPOptions.fatLoachs, new Vector2(margin, lineCount))
					{description = dsc},
					new OpLabel(critBox12.pos.x + xPad, critBox12.pos.y + yPad, dsc)
					{description = dsc}
				});
			}





			//------------------ ANOTHER NEW TAB FOR OTHER STUFF

			int infoTab = 3;
			lineCount = 550;

			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("Enable the Improved Input Config mod to change keybinds")));

			lineCount -= 25;
			Tabs[infoTab].AddItems(new OpLabel(35, lineCount, BPTranslate("If you run into any bugs or issues, let me know so I can fix them!")));

			lineCount -= 35;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("Message me on discord: WillowWisp#3565 ")));
			lineCount -= 25;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("Or ping @WillowWisp on the rain world Discord Server in the modding-support channel")));
			lineCount -= 25;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("Or leave a comment on the mod workshop page in the Bug Reporting discussion!")));
			lineCount -= 25;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("Or anywhere you want please I'm begging and sobbing please report your bugs")));

			lineCount -= 50;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("Super bonus points if you include your error log files (if they generated)")));
			lineCount -= 25;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("located in: /Program Files (86)/Steam/steamapps/common/Rain World/exceptionLog.txt")));


			lineCount -= 50;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("Leave feedback on the mod page and rate the mod if you enjoy it!")));
			lineCount -= 25;
			Tabs[infoTab].AddItems(new OpLabel(35f, lineCount, BPTranslate("If you post footage, send me a link! I'd love to see! :D")));

		}

	}*/
    #endregion

    #region Menu.Remix.MixedUI
    /*
	namespace Menu.Remix.MixedUI
	{
		// Token: 0x0200060A RID: 1546
		public class OpComboBox__ : UIconfig, ICanBeTyped
		{
			// Token: 0x06004092 RID: 16530 RVA: 0x004AFC84 File Offset: 0x004ADE84
			public OpComboBox__(Configurable<string> config, Vector2 pos, float width, List<ListItem> list) : this(config, pos, width, list)
			{
			}

			// Token: 0x06004093 RID: 16531 RVA: 0x004AFC91 File Offset: 0x004ADE91
			public OpComboBox__(Configurable<string> config, Vector2 pos, float width, string[] array) : this(config, pos, width, OpComboBox._ArrayToList(array))
			{
			}

			// Token: 0x06004094 RID: 16532 RVA: 0x004AFCA4 File Offset: 0x004ADEA4
			protected static List<ListItem> _ArrayToList(string[] array)
			{
				List<ListItem> list = new List<ListItem>();
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(new ListItem(array[i], i));
				}
				return list;
			}

			// Token: 0x06004095 RID: 16533 RVA: 0x004AFCD5 File Offset: 0x004ADED5
			public OpComboBox__(Configurable<string> config, Vector2 pos, float width) : this(config, pos, width, OpComboBox._InfoToList(config))
			{
			}

			// Token: 0x06004096 RID: 16534 RVA: 0x004AFCE8 File Offset: 0x004ADEE8
			protected static List<ListItem> _InfoToList(Configurable<string> config)
			{
				if (config == null || config.info == null || config.info.acceptable == null || !(config.info.acceptable is ConfigAcceptableList<string>))
				{
					throw new ElementFormatException("To use this constructor, Configurable<string> must have ConfigurableInfo with ConfigAccpetableList.");
				}
				ConfigAcceptableList<string> configAcceptableList = config.info.acceptable as ConfigAcceptableList<string>;
				List<ListItem> list = new List<ListItem>();
				for (int i = 0; i < configAcceptableList.AcceptableValues.Length; i++)
				{
					list.Add(new ListItem(configAcceptableList.AcceptableValues[i], i));
				}
				return list;
			}

			// Token: 0x06004097 RID: 16535 RVA: 0x004AFD6C File Offset: 0x004ADF6C
			internal OpComboBox__(ConfigurableBase configBase, Vector2 pos, float width, List<ListItem> list) : base(configBase, pos, new Vector2(width, 24f))
			{
				this._size = new Vector2(Mathf.Max(30f, base.size.x), 24f);
				this.fixedSize = new Vector2?(new Vector2(-1f, 24f));
				if (this._IsResourceSelector)
				{
					return;
				}
				if (list == null || list.Count < 1)
				{
					throw new ElementFormatException(this, "The list must contain at least one ListItem", base.Key);
				}
				list.Sort(new Comparison<ListItem>(ListItem.Comparer));
				this._itemList = list.ToArray();
				this._ResetIndex();
				this._Initialize(base.defaultValue);
			}

			// Token: 0x06004098 RID: 16536 RVA: 0x004AFE70 File Offset: 0x004AE070
			protected internal override string DisplayDescription()
			{
				if (base.MenuMouseMode)
				{
					if (!this.held)
					{
						if (!string.IsNullOrEmpty(this.description))
						{
							return this.description;
						}
						return OptionalText.GetText(OptionalText.ID.OpComboBox_MouseOpenTuto);
					}
					else
					{
						if (this._listHover >= 0)
						{
							string text = "";
							if (this._searchMode)
							{
								if (this._listTop + this._listHover < this._searchList.Count)
								{
									text = this._searchList[this._listTop + this._listHover].desc;
								}
							}
							else if (this._listTop + this._listHover < this._itemList.Length)
							{
								text = this._itemList[this._listTop + this._listHover].desc;
							}
							if (!string.IsNullOrEmpty(text))
							{
								return text;
							}
						}
						if (!string.IsNullOrEmpty(this.description))
						{
							return this.description;
						}
						if (this._searchMode)
						{
							return OptionalText.GetText(OptionalText.ID.OpComboBox_MouseSearchTuto);
						}
						return OptionalText.GetText(OptionalText.ID.OpComboBox_MouseUseTuto);
					}
				}
				else if (!this.held)
				{
					if (!string.IsNullOrEmpty(this.description))
					{
						return this.description;
					}
					return OptionalText.GetText(OptionalText.ID.OpComboBox_NonMouseOpenTuto);
				}
				else
				{
					if (this._listHover >= 0)
					{
						string text2 = "";
						if (this._searchMode)
						{
							if (this._listTop + this._listHover < this._searchList.Count)
							{
								text2 = this._searchList[this._listTop + this._listHover].desc;
							}
						}
						else if (this._listTop + this._listHover < this._itemList.Length)
						{
							text2 = this._itemList[this._listTop + this._listHover].desc;
						}
						if (!string.IsNullOrEmpty(text2))
						{
							return text2;
						}
					}
					if (!string.IsNullOrEmpty(this.description))
					{
						return this.description;
					}
					return OptionalText.GetText(OptionalText.ID.OpComboBox_NonMouseUseTuto);
				}
			}

			// Token: 0x06004099 RID: 16537 RVA: 0x004B0038 File Offset: 0x004AE238
			protected void _ResetIndex()
			{
				for (int i = 0; i < this._itemList.Length; i++)
				{
					this._itemList[i].index = i;
				}
				this._searchDelay = Mathf.FloorToInt(Custom.LerpMap((float)Mathf.Clamp(this._itemList.Length, 10, 90), 10f, 90f, 10f, 50f));
				this._searchDelay = UIelement.FrameMultiply(this._searchDelay);
				if (ModManager.MMF)
				{
					if (Custom.rainWorld.options.quality == Options.Quality.HIGH)
					{
						this._searchDelay = 10;
						return;
					}
					if (Custom.rainWorld.options.quality == Options.Quality.MEDIUM)
					{
						this._searchDelay = Math.Max(10, this._searchDelay / 2);
					}
				}
			}

			// Token: 0x0600409A RID: 16538 RVA: 0x004B010C File Offset: 0x004AE30C
			protected virtual void _Initialize(string defaultName)
			{
				this.OnKeyDown = (Action<char>)Delegate.Combine(this.OnKeyDown, new Action<char>(this.KeyboardAccept));
				this.Assign();
				this.mouseOverStopsScrollwheel = true;
				if (string.IsNullOrEmpty(defaultName))
				{
					this.allowEmpty = true;
					base.defaultValue = "";
					this._value = "";
				}
				else
				{
					bool flag = false;
					for (int i = 0; i < this._itemList.Length; i++)
					{
						if (this._itemList[i].name == defaultName)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						base.defaultValue = defaultName;
						this._value = base.defaultValue;
					}
					else
					{
						base.defaultValue = this._itemList[0].name;
						this._value = this._itemList[0].name;
					}
				}
				this._rect = new DyeableRect(this.myContainer, Vector2.zero, base.size, true);
				this._lblText = UIelement.FLabelCreate("", false);
				this._lblText.text = LabelTest.TrimText(string.IsNullOrEmpty(this.value) ? this.DASHES : this._GetDisplayValue(), base.size.x - 32f, true, false);
				this._lblText.alignment = FLabelAlignment.Left;
				this._lblText.x = 12f;
				this._lblText.y = base.size.y / 2f;
				this.myContainer.AddChild(this._lblText);
				if (this._IsListBox)
				{
					this._neverOpened = false;
					this._rectList = new DyeableRect(this.myContainer, Vector2.zero, base.size, true);
					this._rectScroll = new DyeableRect(this.myContainer, Vector2.zero, Vector2.one * 15f, true);
					this._glowFocus = new GlowGradient(this.myContainer, Vector2.zero, new Vector2((base.size.x - 25f) / 2f, 15f), 0.5f)
					{
						color = this.colorEdge
					};
					this._glowFocus.Hide();
				}
				else
				{
					this._neverOpened = true;
					this._sprArrow = new FSprite("Big_Menu_Arrow", true)
					{
						scale = 0.5f,
						rotation = 180f,
						anchorX = 0.5f,
						anchorY = 0.5f
					};
					this.myContainer.AddChild(this._sprArrow);
					this._sprArrow.SetPosition(base.size.x - 12f, base.size.y / 2f);
				}
				this._lblList = new FLabel[0];
				this._searchCursor = new FSprite("modInputCursor", true);
				this.myContainer.AddChild(this._searchCursor);
				this._searchCursor.isVisible = false;
				this._bumpList = new BumpBehaviour(this)
				{
					held = false,
					Focused = false
				};
				this._bumpScroll = new BumpBehaviour(this)
				{
					held = false,
					Focused = false
				};
				this.GrafUpdate(0f);
			}

			// Token: 0x0600409B RID: 16539 RVA: 0x004B0444 File Offset: 0x004AE644
			public ListItem[] GetItemList()
			{
				return this._itemList;
			}

			// Token: 0x17000A82 RID: 2690
			// (get) Token: 0x0600409C RID: 16540 RVA: 0x004B044C File Offset: 0x004AE64C
			protected bool _IsResourceSelector
			{
				get
				{
					return this is OpResourceSelector || this is OpResourceList;
				}
			}

			// Token: 0x17000A83 RID: 2691
			// (get) Token: 0x0600409D RID: 16541 RVA: 0x004B0461 File Offset: 0x004AE661
			protected bool _IsListBox
			{
				get
				{
					return this is OpListBox;
				}
			}

			// Token: 0x0600409E RID: 16542 RVA: 0x004B046C File Offset: 0x004AE66C
			public override void Reset()
			{
				if (this.held)
				{
					this._CloseList();
				}
				base.Reset();
			}

			// Token: 0x17000A84 RID: 2692
			// (get) Token: 0x0600409F RID: 16543 RVA: 0x004B0482 File Offset: 0x004AE682
			// (set) Token: 0x060040A0 RID: 16544 RVA: 0x004B048C File Offset: 0x004AE68C
			public override string value
			{
				get
				{
					return base.value;
				}
				set
				{
					if (base.value != value)
					{
						if (string.IsNullOrEmpty(value))
						{
							base.value = (this.allowEmpty ? "" : this._itemList[0].name);
							return;
						}
						ListItem[] itemList = this._itemList;
						for (int i = 0; i < itemList.Length; i++)
						{
							if (itemList[i].name == value)
							{
								base.value = value;
								return;
							}
						}
					}
				}
			}

			// Token: 0x17000A85 RID: 2693
			// (get) Token: 0x060040A1 RID: 16545 RVA: 0x004B0507 File Offset: 0x004AE707
			// (set) Token: 0x060040A2 RID: 16546 RVA: 0x004B050F File Offset: 0x004AE70F
			public bool allowEmpty { get; private set; }

			// Token: 0x060040A3 RID: 16547 RVA: 0x004B0518 File Offset: 0x004AE718
			public void SetAllowEmpty()
			{
				if (string.IsNullOrEmpty(this.cfgEntry.defaultValue))
				{
					if (!this.allowEmpty && this.value == this._itemList[0].name)
					{
						this._value = "";
					}
					base.defaultValue = "";
				}
				this.allowEmpty = true;
			}

			// Token: 0x060040A4 RID: 16548 RVA: 0x004B057C File Offset: 0x004AE77C
			public override void GrafUpdate(float timeStacker)
			{
				base.GrafUpdate(timeStacker);
				base.bumpBehav.greyedOut = this.greyedOut;
				this._rect.colorEdge = base.bumpBehav.GetColor(this.colorEdge);
				this._rect.fillAlpha = base.bumpBehav.FillAlpha;
				this._rect.addSize = new Vector2(4f, 4f) * base.bumpBehav.AddSize;
				this._rect.colorFill = (this.greyedOut ? base.bumpBehav.GetColor(this.colorFill) : this.colorFill);
				this._rect.GrafUpdate(timeStacker);
				Color color = this.held ? MenuColorEffect.MidToDark(this._rect.colorEdge) : this._rect.colorEdge;
				if (!this._IsListBox)
				{
					this._sprArrow.color = color;
					this._lblText.color = (this._searchMode ? this._rect.colorEdge : color);
				}
				this._lblText.color = (this._searchMode ? this._rect.colorEdge : color);
				if (!this._IsListBox && !this.held)
				{
					this._lblText.text = LabelTest.TrimText(string.IsNullOrEmpty(this.value) ? this.DASHES : this._GetDisplayValue(), base.size.x - 32f, true, false);
					return;
				}
				this._rectList.size.x = base.size.x;
				this._rectList.pos = new Vector2(0f, this._downward ? (-this._rectList.size.y) : base.size.y);
				if (this._IsListBox && this._downward)
				{
					DyeableRect rectList = this._rectList;
					rectList.pos.y = rectList.pos.y + this._rectList.size.y;
				}
				this._rectList.addSize = new Vector2(4f, 4f) * this._bumpList.AddSize;
				this._rectList.colorEdge = this._bumpList.GetColor(this.colorEdge);
				this._rectList.colorFill = this.colorFill;
				this._rectList.fillAlpha = (this._IsListBox ? this._bumpList.FillAlpha : Mathf.Lerp(0.5f, 0.7f, this._bumpList.col));
				this._rectList.GrafUpdate(timeStacker);
				for (int i = 0; i < this._lblList.Length; i++)
				{
					string text = this._searchMode ? ((this._searchList.Count > this._listTop + i) ? this._searchList[this._listTop + i].displayName : "") : this._itemList[this._listTop + i].displayName;
					this._lblList[i].text = LabelTest.TrimText(text, base.size.x - 36f, true, false);
					this._lblList[i].color = ((text == this.value) ? MenuColorEffect.MidToDark(this._rect.colorEdge) : this._rectList.colorEdge);
					if (i == this._listHover)
					{
						this._lblList[i].color = Color.Lerp(this._lblList[i].color, (this._mouseDown || text == this.value) ? MenuColorEffect.MidToDark(this._lblList[i].color) : Color.white, this._bumpList.Sin((text == this._GetDisplayValue()) ? 60f : 10f));
					}
					this._lblList[i].x = 12f;
					this._lblList[i].y = -15f - 20f * (float)i + (this._downward ? 0f : (base.size.y + this._rectList.size.y));
					if (this._IsListBox && this._downward)
					{
						this._lblList[i].y += this._rectList.size.y;
					}
				}
				if (this._glowFocus != null)
				{
					if (this._listHover >= 0 && (this._MouseOverList() || (!base.MenuMouseMode && this.held)))
					{
						this._glowFocus.Show();
						this._glowFocus.pos = this._lblList[Math.Min(this._listHover, this._lblList.Length - 1)].GetPosition() - new Vector2(0f, this._glowFocus.radV);
						this._glowFocus.color = Color.Lerp(this._rect.colorEdge, Color.white, 0.6f);
						this._glowFocus.alpha = Mathf.Lerp(0.2f, 0.6f, this._bumpList.Sin(10f));
					}
					else
					{
						this._glowFocus.Hide();
					}
				}
				this._lblText.text = LabelTest.TrimText(this._searchMode ? this._searchQuery : (string.IsNullOrEmpty(this.value) ? this.DASHES : this._GetDisplayValue()), base.size.x - 32f, true, false);
				if (this._searchMode)
				{
					this._searchCursor.x = Mathf.Min(base.size.x - 24f, 12f + LabelTest.GetWidth(this._searchQuery, false) + LabelTest.CharMean(false));
					this._searchCursor.y = base.size.y * 0.5f + ((this._IsListBox && this._downward) ? this._rectList.size.y : 0f);
					this._searchCursor.color = Color.Lerp(this.colorEdge, MenuColorEffect.MidToDark(this.colorEdge), base.bumpBehav.Sin((this._searchList.Count > 0) ? 10f : 30f));
					this._searchCursor.alpha = 0.4f + 0.6f * Mathf.Clamp01((float)this._searchIdle / (float)this._searchDelay);
				}
				int num = this._searchMode ? this._searchList.Count : this._itemList.Length;
				if (num > this._lblList.Length)
				{
					this._rectScroll.Show();
					this._rectScroll.pos.x = this._rectList.pos.x + this._rectList.size.x - 20f;
					if (!Mathf.Approximately(this._rectScroll.size.y, this._ScrollLen(num)))
					{
						this._rectScroll.size.y = this._ScrollLen(num);
						this._rectScroll.pos.y = this._ScrollPos(num);
					}
					else
					{
						this._rectScroll.pos.y = Custom.LerpAndTick(this._rectScroll.pos.y, this._ScrollPos(num), this._bumpScroll.held ? 0.6f : 0.2f, (this._bumpScroll.held ? 0.6f : 0.2f) / UIelement.frameMulti);
					}
					this._rectScroll.addSize = new Vector2(2f, 2f) * this._bumpScroll.AddSize;
					this._rectScroll.colorEdge = this._bumpScroll.GetColor(this.colorEdge);
					this._rectScroll.colorFill = (this._bumpScroll.held ? this._rectScroll.colorEdge : this.colorFill);
					this._rectScroll.fillAlpha = (this._bumpScroll.held ? 1f : this._bumpScroll.FillAlpha);
					this._rectScroll.GrafUpdate(timeStacker);
					return;
				}
				this._rectScroll.Hide();
			}

			// Token: 0x17000A86 RID: 2694
			// (get) Token: 0x060040A5 RID: 16549 RVA: 0x004B0DFB File Offset: 0x004AEFFB
			protected int _listHeight
			{
				get
				{
					return Custom.IntClamp((int)this.listHeight, 1, this._itemList.Length);
				}
			}

			// Token: 0x060040A6 RID: 16550 RVA: 0x004B0E14 File Offset: 0x004AF014
			protected float _ScrollPos(int listSize)
			{
				return this._rectList.pos.y + 10f + (this._rectList.size.y - 20f - this._rectScroll.size.y) * (float)(listSize - this._lblList.Length - this._listTop) / (float)(listSize - this._lblList.Length);
			}

			// Token: 0x060040A7 RID: 16551 RVA: 0x004B0E7E File Offset: 0x004AF07E
			protected float _ScrollLen(int listSize)
			{
				return (this._rectList.size.y - 40f) * Mathf.Clamp01((float)this._lblList.Length / (float)listSize) + 20f;
			}

			// Token: 0x060040A8 RID: 16552 RVA: 0x004B0EB0 File Offset: 0x004AF0B0
			public int GetIndex(string checkName = "")
			{
				if (string.IsNullOrEmpty(checkName))
				{
					checkName = this.value;
				}
				for (int i = 0; i < this._itemList.Length; i++)
				{
					if (this._itemList[i].name == checkName)
					{
						return this._itemList[i].index;
					}
				}
				return -1;
			}

			// Token: 0x17000A87 RID: 2695
			// (get) Token: 0x060040A9 RID: 16553 RVA: 0x004B0F0C File Offset: 0x004AF10C
			// (set) Token: 0x060040AA RID: 16554 RVA: 0x004B0F14 File Offset: 0x004AF114
			public Action<char> OnKeyDown { get; set; }

			// Token: 0x060040AB RID: 16555 RVA: 0x004B0F20 File Offset: 0x004AF120
			public void KeyboardAccept(char input)
			{
				if (!this.held || !this._searchMode || this._bumpScroll.held)
				{
					return;
				}
				if (input == '\b')
				{
					if (this._searchQuery.Length > 0)
					{
						this._searchQuery = this._searchQuery.Substring(0, this._searchQuery.Length - 1);
						this._searchIdle = -1;
						base.PlaySound(SoundID.MENY_Already_Selected_MultipleChoice_Clicked);
						return;
					}
				}
				else if (char.IsLetterOrDigit(input) || input == ' ')
				{
					base.bumpBehav.flash = 2.5f;
					this._searchQuery += input.ToString();
					this._searchIdle = -1;
					base.PlaySound(SoundID.MENU_Checkbox_Uncheck);
				}
			}

			// Token: 0x060040AC RID: 16556 RVA: 0x004B0FD8 File Offset: 0x004AF1D8
			protected void _SearchModeUpdate()
			{
				base.ForceMenuMouseMode(new bool?(true));
				if (this._inputDelay == 0 && this._searchIdle < UIelement.FrameMultiply(this._searchDelay))
				{
					this._searchIdle++;
					if (this._searchIdle == this._searchDelay)
					{
						this._RefreshSearchList();
					}
				}
			}

			// Token: 0x060040AD RID: 16557 RVA: 0x004B1030 File Offset: 0x004AF230
			public override void Update()
			{
				base.Update();
				if (this._dTimer > 0)
				{
					this._dTimer--;
				}
				this._rect.Update();
				DyeableRect rectList = this._rectList;
				if (rectList != null)
				{
					rectList.Update();
				}
				DyeableRect rectScroll = this._rectScroll;
				if (rectScroll != null)
				{
					rectScroll.Update();
				}
				if (this.greyedOut)
				{
					DyeableRect rectList2 = this._rectList;
					if (rectList2 != null && !rectList2.isHidden)
					{
						this._mouseDown = false;
						this.held = false;
						if (!this._IsListBox)
						{
							this._CloseList();
						}
					}
					return;
				}
				if (base.MenuMouseMode)
				{
					this._MouseModeUpdate();
					return;
				}
				this._NonMouseModeUpdate();
			}

			// Token: 0x060040AE RID: 16558 RVA: 0x004B10D8 File Offset: 0x004AF2D8
			protected virtual void _MouseModeUpdate()
			{
				base.bumpBehav.Focused = (base.MouseOver && !this._MouseOverList());
				if (this.held)
				{
					this._bumpList.Focused = this._MouseOverList();
					this._bumpScroll.Focused = (base.MousePos.x >= this._rectScroll.pos.x && base.MousePos.x <= this._rectScroll.pos.x + this._rectScroll.size.x);
					this._bumpScroll.Focused = (this._bumpScroll.Focused && base.MousePos.y >= this._rectScroll.pos.y && base.MousePos.y <= this._rectScroll.pos.y + this._rectScroll.size.y);
					if (this._searchMode && !this._bumpScroll.held)
					{
						this._SearchModeUpdate();
					}
					int num = this._searchMode ? this._searchList.Count : this._itemList.Length;
					if (!this._bumpScroll.held)
					{
						if (this.MouseOver)
						{
							if (Input.GetMouseButton(0) && !this._mouseDown)
							{
								if (this._bumpScroll.Focused && num > this._lblList.Length)
								{
									this._scrollHeldPos = base.MousePos.y - this._rectScroll.pos.y + base.pos.y;
									this._bumpScroll.held = true;
									base.PlaySound(SoundID.MENU_First_Scroll_Tick);
								}
								else
								{
									this._mouseDown = true;
								}
							}
							if (!this._MouseOverList())
							{
								if (Input.GetMouseButton(0) || !this._mouseDown)
								{
									goto IL_60F;
								}
								this._mouseDown = false;
								if (this._dTimer > 0)
								{
									this._dTimer = 0;
									this._searchMode = true;
									this._EnterSearchMode();
									return;
								}
								this._dTimer = UIelement.FrameMultiply(15);
								if (this.allowEmpty)
								{
									this.value = "";
								}
								base.PlaySound(SoundID.MENU_Checkbox_Uncheck);
							}
							else
							{
								if (base.MousePos.x >= 10f && base.MousePos.x <= this._rectList.size.x - 30f)
								{
									if (this._downward)
									{
										this._listHover = Mathf.FloorToInt((base.MousePos.y + this._rectList.size.y + 10f) / 20f);
									}
									else
									{
										this._listHover = Mathf.FloorToInt((base.MousePos.y - base.size.y + 10f) / 20f);
									}
									if (this._listHover > this._lblList.Length || this._listHover <= 0)
									{
										this._listHover = -1;
									}
									else
									{
										this._listHover = this._lblList.Length - this._listHover;
									}
								}
								else
								{
									this._listHover = -1;
								}
								if (base.Menu.mouseScrollWheelMovement != 0)
								{
									int num2 = this._listTop + (int)Mathf.Sign((float)base.Menu.mouseScrollWheelMovement) * Mathf.CeilToInt((float)this._lblList.Length / 2f);
									num2 = Custom.IntClamp(num2, 0, num - this._lblList.Length);
									if (this._listTop != num2)
									{
										base.PlaySound(SoundID.MENU_Scroll_Tick);
										this._listTop = num2;
										this._bumpScroll.flash = Mathf.Min(1f, this._bumpScroll.flash + 0.2f);
										this._bumpScroll.sizeBump = Mathf.Min(2.5f, this._bumpScroll.sizeBump + 0.3f);
										goto IL_60F;
									}
									goto IL_60F;
								}
								else
								{
									if (Input.GetMouseButton(0) || !this._mouseDown)
									{
										goto IL_60F;
									}
									this._mouseDown = false;
									if (this._listHover < 0)
									{
										goto IL_60F;
									}
									string text = this.value;
									if (this._searchMode)
									{
										if (this._listTop + this._listHover < this._searchList.Count)
										{
											text = this._searchList[this._listTop + this._listHover].name;
										}
									}
									else if (this._listTop + this._listHover < this._itemList.Length)
									{
										text = this._itemList[this._listTop + this._listHover].name;
									}
									if (!(text != this.value))
									{
										base.PlaySound(SoundID.MENY_Already_Selected_MultipleChoice_Clicked);
										goto IL_60F;
									}
									this.value = text;
									base.PlaySound(SoundID.MENU_MultipleChoice_Clicked);
								}
							}
						}
						else
						{
							if ((!Input.GetMouseButton(0) || this._mouseDown) && (!this._mouseDown || Input.GetMouseButton(0)))
							{
								goto IL_60F;
							}
							base.PlaySound(SoundID.MENU_Checkbox_Uncheck);
						}
						this._mouseDown = false;
						this.held = false;
						this._CloseList();
						return;
					}
					if (Input.GetMouseButton(0))
					{
						int num3 = Mathf.RoundToInt((base.MousePos.y - this._rectList.pos.y + base.pos.y - 10f - this._scrollHeldPos) * (float)(num - this._lblList.Length) / (this._rectList.size.y - 20f - this._rectScroll.size.y));
						num3 = Custom.IntClamp(num - this._lblList.Length - num3, 0, num - this._lblList.Length);
						if (this._listTop != num3)
						{
							base.PlaySound(SoundID.MENU_Scroll_Tick);
							this._listTop = num3;
							this._bumpScroll.flash = Mathf.Min(1f, this._bumpScroll.flash + 0.2f);
							this._bumpScroll.sizeBump = Mathf.Min(2.5f, this._bumpScroll.sizeBump + 0.3f);
						}
					}
					else
					{
						this._bumpScroll.held = false;
						this._mouseDown = false;
						base.PlaySound(SoundID.MENU_Scroll_Tick);
					}
				IL_60F:
					this._bumpList.Update();
					this._bumpScroll.Update();
					return;
				}
				if (this.greyedOut)
				{
					return;
				}
				if (base.MouseOver)
				{
					if (Input.GetMouseButton(0))
					{
						this._mouseDown = true;
						return;
					}
					if (this._mouseDown)
					{
						this._mouseDown = false;
						if (this._dTimer > 0)
						{
							this._dTimer = 0;
							this._searchMode = true;
							this._EnterSearchMode();
						}
						else
						{
							this._dTimer = UIelement.FrameMultiply(15);
						}
						this.held = true;
						this.fixedSize = new Vector2?(base.size);
						this._OpenList();
						base.PlaySound(SoundID.MENU_Checkbox_Check);
						return;
					}
					if (base.Menu.mouseScrollWheelMovement != 0)
					{
						int index = this.GetIndex("");
						int num4 = index + (int)Mathf.Sign((float)base.Menu.mouseScrollWheelMovement);
						num4 = Custom.IntClamp(num4, 0, this._itemList.Length - 1);
						if (num4 != index)
						{
							base.bumpBehav.flash = 1f;
							base.PlaySound(SoundID.MENU_Scroll_Tick);
							base.bumpBehav.sizeBump = Mathf.Min(2.5f, base.bumpBehav.sizeBump + 1f);
							this.value = this._itemList[num4].name;
							return;
						}
					}
				}
				else if (!Input.GetMouseButton(0))
				{
					this._mouseDown = false;
				}
			}

			// Token: 0x060040AF RID: 16559 RVA: 0x004B1860 File Offset: 0x004AFA60
			protected virtual void _NonMouseModeUpdate()
			{
				base.bumpBehav.Focused = (base.Focused && !this.held);
				if (this.held)
				{
					this._bumpList.Focused = true;
					if (!base.bumpBehav.ButtonPress(BumpBehaviour.ButtonType.Throw))
					{
						OpComboBox.<> c__DisplayClass66_0 CS$<> 8__locals1;
						CS$<> 8__locals1.listSize = (this._searchMode ? this._searchList.Count : this._itemList.Length);
						if (base.CtlrInput.y != 0)
						{
							OpComboBox.<> c__DisplayClass66_1 CS$<> 8__locals2;
							CS$<> 8__locals2.dir = base.bumpBehav.JoystickPressAxis(true);
							if (CS$<> 8__locals2.dir != 0)
							{
								this.< _NonMouseModeUpdate > g___ScrollTick | 66_0(true, ref CS$<> 8__locals1, ref CS$<> 8__locals2);
							}

							else
							{
								CS$<> 8__locals2.dir = base.bumpBehav.JoystickHeldAxis(true, 2f);
								if (CS$<> 8__locals2.dir != 0)
								{
									this.< _NonMouseModeUpdate > g___ScrollTick | 66_0(false, ref CS$<> 8__locals1, ref CS$<> 8__locals2);
								}
							}
						}
						if (base.bumpBehav.ButtonPress(BumpBehaviour.ButtonType.Jump))
						{
							string text = this.value;
							if (this._searchMode)
							{
								if (this._listTop + this._listHover < this._searchList.Count)
								{
									text = this._searchList[this._listTop + this._listHover].name;
								}
							}
							else if (this._listTop + this._listHover < this._itemList.Length)
							{
								text = this._itemList[this._listTop + this._listHover].name;
							}
							if (text != this.value)
							{
								this.value = text;
								base.PlaySound(SoundID.MENU_MultipleChoice_Clicked);
								goto IL_19E;
							}
							base.PlaySound(SoundID.MENY_Already_Selected_MultipleChoice_Clicked);
						}
						this._bumpList.Update();
						this._bumpScroll.Update();
						return;
					}
				IL_19E:
					this.held = false;
					if (!this._IsListBox)
					{
						this._CloseList();
					}
					return;
				}
			}

			// Token: 0x060040B0 RID: 16560 RVA: 0x004B1A24 File Offset: 0x004AFC24
			protected internal override void NonMouseSetHeld(bool newHeld)
			{
				base.NonMouseSetHeld(newHeld);
				if (newHeld)
				{
					if (!this._IsListBox)
					{
						this._OpenList();
						base.PlaySound(SoundID.MENU_Checkbox_Check);
					}
					this._listHover = Custom.IntClamp(this.GetIndex("") - this._listTop, 0, this._lblList.Length - 1);
					return;
				}
				if (!this._IsListBox)
				{
					this._CloseList();
				}
			}

			// Token: 0x060040B1 RID: 16561 RVA: 0x004B1A8C File Offset: 0x004AFC8C
			protected internal override void Change()
			{
				base.Change();
				this._size.x = Mathf.Max(30f, this._size.x);
				this._rect.size = base.size;
				this._lblText.x = 12f;
				this._lblText.y = base.size.y / 2f;
				if (this._glowFocus != null)
				{
					this._glowFocus.radH = (this._size.x - 25f) / 2f;
				}
				if (this._IsListBox && this._downward)
				{
					this._rect.pos.y = this._rectList.size.y;
					this._lblText.y += this._rectList.size.y;
				}
				else
				{
					this._rect.pos.y = 0f;
				}
				if (!this._IsListBox)
				{
					this._sprArrow.SetPosition(base.size.x - 12f, base.size.y / 2f);
				}
			}

			// Token: 0x060040B2 RID: 16562 RVA: 0x004B1BC4 File Offset: 0x004AFDC4
			protected internal override void Deactivate()
			{
				base.Deactivate();
				if (this._IsListBox)
				{
					this._searchMode = false;
					this._bumpScroll.held = false;
					return;
				}
				this._CloseList();
			}

			// Token: 0x14000016 RID: 22
			// (add) Token: 0x060040B3 RID: 16563 RVA: 0x004B1BF0 File Offset: 0x004AFDF0
			// (remove) Token: 0x060040B4 RID: 16564 RVA: 0x004B1C28 File Offset: 0x004AFE28
			public event OnSignalHandler OnListOpen;

			// Token: 0x14000017 RID: 23
			// (add) Token: 0x060040B5 RID: 16565 RVA: 0x004B1C60 File Offset: 0x004AFE60
			// (remove) Token: 0x060040B6 RID: 16566 RVA: 0x004B1C98 File Offset: 0x004AFE98
			public event OnSignalHandler OnListClose;

			// Token: 0x060040B7 RID: 16567 RVA: 0x004B1CD0 File Offset: 0x004AFED0
			protected void _OpenList()
			{
				float num = 20f * (float)Mathf.Clamp(this._itemList.Length, 1, this._listHeight) + 10f;
				if (!this._IsListBox)
				{
					OnSignalHandler onListOpen = this.OnListOpen;
					if (onListOpen != null)
					{
						onListOpen(this);
					}
					if (num < base.GetPos().y)
					{
						this._downward = true;
					}
					else if (100f < base.GetPos().y)
					{
						this._downward = true;
						num = 100f;
					}
					else
					{
						float num2 = 600f;
						if (base.InScrollBox)
						{
							num2 = (base.scrollBox.horizontal ? base.scrollBox.size.y : base.scrollBox.contentSize);
						}
						num2 -= base.GetPos().y + base.size.y;
						if (num2 < base.GetPos().y)
						{
							this._downward = true;
							num = Mathf.Floor(base.GetPos().y / 20f) * 20f - 10f;
						}
						else
						{
							this._downward = false;
							num = Mathf.Min(num, Mathf.Clamp(Mathf.Floor(num2 / 20f), 1f, (float)this._listHeight) * 20f + 10f);
						}
					}
					if (this._neverOpened)
					{
						this._rectList = new DyeableRect(this.myContainer, Vector2.zero, base.size, true)
						{
							fillAlpha = 0.95f
						};
						this._rectScroll = new DyeableRect(this.myContainer, Vector2.zero, 15f * Vector2.one, true);
						this._glowFocus = new GlowGradient(this.myContainer, Vector2.zero, new Vector2((base.size.x - 25f) / 2f, 15f), 0.5f)
						{
							color = this.colorEdge
						};
						this._neverOpened = false;
					}
					this._sprArrow.rotation = (this._downward ? 180f : 0f);
				}
				this._rectList.Show();
				this._rectList.size = new Vector2(base.size.x, num);
				this._rectList.pos = new Vector2(0f, this._downward ? (-this._rectList.size.y) : base.size.y);
				if (this._IsListBox && this._downward)
				{
					DyeableRect rectList = this._rectList;
					rectList.pos.y = rectList.pos.y + this._rectList.size.y;
					this._rect.pos.y = this._rectList.size.y;
					this._lblText.y += this._rectList.size.y;
				}
				this._glowFocus.Show();
				this._glowFocus.radH = (base.size.x - 25f) / 2f;
				this._lblList = new FLabel[Mathf.FloorToInt(num / 20f)];
				if (this._downward)
				{
					this._listTop = this.GetIndex("") + 1;
					if (this._listTop > this._itemList.Length - this._lblList.Length)
					{
						this._listTop = this._itemList.Length - this._lblList.Length;
					}
				}
				else
				{
					this._listTop = this.GetIndex("") - this._lblList.Length;
				}
				if (this._listTop < 0)
				{
					this._listTop = 0;
				}
				for (int i = 0; i < this._lblList.Length; i++)
				{
					this._lblList[i] = UIelement.FLabelCreate("", false);
					this._lblList[i].text = this._itemList[this._listTop + i].EffectiveDisplayName;
					this._lblList[i].alignment = FLabelAlignment.Left;
					this.myContainer.AddChild(this._lblList[i]);
				}
				if (this._lblList.Length < this._itemList.Length)
				{
					this._rectScroll.Show();
					this._rectScroll.size = new Vector2(15f, this._ScrollLen(this._itemList.Length));
					this._rectScroll.pos = new Vector2(this._rectList.pos.x + this._rectList.size.x - 20f, this._ScrollPos(this._itemList.Length));
				}
				else
				{
					this._rectScroll.Hide();
				}
				base.bumpBehav.flash = 1f;
				this._bumpList.flash = 1f;
				this._bumpList.held = false;
				this._bumpScroll.held = false;
				if (base.InScrollBox && !this._IsListBox)
				{
					base.scrollBox.ScrollToRect(new Rect(base.PosX, base.PosY - (this._downward ? this._rectList.size.y : 0f), base.size.x, base.size.y + this._rectList.size.y), false);
				}
			}

			// Token: 0x060040B8 RID: 16568 RVA: 0x004B2224 File Offset: 0x004B0424
			private void _CloseList()
			{
				OnSignalHandler onListClose = this.OnListClose;
				if (onListClose != null)
				{
					onListClose(this);
				}
				this._searchMode = false;
				this._searchCursor.isVisible = false;
				this.fixedSize = null;
				if (!this._neverOpened)
				{
					this._rectList.Hide();
					this._rectScroll.Hide();
					this._glowFocus.Hide();
				}
				for (int i = 0; i < this._lblList.Length; i++)
				{
					this._lblList[i].isVisible = false;
					this._lblList[i].RemoveFromContainer();
				}
				this._lblList = new FLabel[0];
				this._bumpScroll.held = false;
			}

			// Token: 0x060040B9 RID: 16569 RVA: 0x004B22D4 File Offset: 0x004B04D4
			protected bool _MouseOverList()
			{
				if (!base.MenuMouseMode)
				{
					return false;
				}
				if (!this.held && !this._IsListBox)
				{
					return false;
				}
				if (base.MousePos.x < 0f || base.MousePos.x > base.size.x)
				{
					return false;
				}
				if (!this._downward)
				{
					return base.MousePos.y >= base.size.y && base.MousePos.y <= base.size.y + this._rectList.size.y;
				}
				if (this._IsListBox)
				{
					return base.MousePos.y >= 0f && base.MousePos.y <= this._rectList.size.y;
				}
				return base.MousePos.y >= -this._rectList.size.y && base.MousePos.y <= 0f;
			}

			// Token: 0x17000A88 RID: 2696
			// (get) Token: 0x060040BA RID: 16570 RVA: 0x004B23EA File Offset: 0x004B05EA
			protected internal override bool MouseOver
			{
				get
				{
					return base.MouseOver || this._MouseOverList();
				}
			}

			// Token: 0x060040BB RID: 16571 RVA: 0x004B23FC File Offset: 0x004B05FC
			public void AddItems(bool sort = true, params ListItem[] newItems)
			{
				if (this._IsResourceSelector)
				{
					throw new InvalidActionException(this, "You cannot use AddItems for OpResourceSelector", base.Key);
				}
				List<ListItem> list = new List<ListItem>(this._itemList);
				list.AddRange(newItems);
				if (sort)
				{
					list.Sort(new Comparison<ListItem>(ListItem.Comparer));
				}
				this._itemList = list.ToArray();
				this._ResetIndex();
				this.Change();
			}

			// Token: 0x060040BC RID: 16572 RVA: 0x004B2464 File Offset: 0x004B0664
			public void RemoveItems(bool selectNext = true, params string[] names)
			{
				if (this._IsResourceSelector)
				{
					throw new InvalidActionException(this, "You cannot use RemoveItems for OpResourceSelector", base.Key);
				}
				List<ListItem> list = new List<ListItem>(this._itemList);
				foreach (string text in names)
				{
					int j = 0;
					while (j < list.Count)
					{
						if (list[j].name == text)
						{
							if (list.Count == 1)
							{
								throw new InvalidActionException(this, "You cannot remove every items in OpComboBox", base.Key);
							}
							if (text == this.value)
							{
								this._value = (selectNext ? list[(j == 0) ? 1 : (j - 1)].name : "");
							}
							list.RemoveAt(j);
							break;
						}
						else
						{
							j++;
						}
					}
				}
				this._itemList = list.ToArray();
				this._ResetIndex();
				this.Change();
			}

			// Token: 0x060040BD RID: 16573 RVA: 0x004B254C File Offset: 0x004B074C
			protected internal override string CopyToClipboard()
			{
				if (!this._IsListBox)
				{
					this._CloseList();
				}
				return base.CopyToClipboard();
			}

			// Token: 0x060040BE RID: 16574 RVA: 0x004B2562 File Offset: 0x004B0762
			protected internal override bool CopyFromClipboard(string value)
			{
				if (!this._searchMode)
				{
					this._searchMode = true;
					this._EnterSearchMode();
				}
				this._searchQuery = value;
				this._RefreshSearchList();
				return true;
			}

			// Token: 0x060040BF RID: 16575 RVA: 0x004B2588 File Offset: 0x004B0788
			protected void _EnterSearchMode()
			{
				this._searchQuery = "";
				this._searchList = new List<ListItem>(this._itemList);
				this._searchList.Sort(new Comparison<ListItem>(ListItem.Comparer));
				this._searchCursor.isVisible = true;
				this._searchCursor.SetPosition(LabelTest.CharMean(false) * 1.5f, base.size.y * 0.5f + ((this._IsListBox && this._downward) ? this._rectList.size.y : 0f));
				this._searchIdle = 1000;
			}

			// Token: 0x060040C0 RID: 16576 RVA: 0x004B2630 File Offset: 0x004B0830
			protected void _RefreshSearchList()
			{
				int num = (this._searchList.Count > 0) ? this.GetIndex(this._searchList[this._listTop].name) : 0;
				this._searchList.Clear();
				for (int i = 0; i < this._itemList.Length; i++)
				{
					if (ListItem.SearchMatch(this._searchQuery, this._itemList[i].displayName) || ListItem.SearchMatch(this._searchQuery, this._itemList[i].name))
					{
						this._searchList.Add(this._itemList[i]);
					}
				}
				this._searchList.Sort(new Comparison<ListItem>(ListItem.Comparer));
				for (int j = 1; j < this._searchList.Count; j++)
				{
					if (num > this.GetIndex(this._searchList[j].name))
					{
						this._listTop = Math.Max(0, j - 1);
						return;
					}
				}
				this._listTop = 0;
			}

			// Token: 0x060040C1 RID: 16577 RVA: 0x004B273C File Offset: 0x004B093C
			protected string _GetDisplayValue()
			{
				foreach (ListItem listItem in this._itemList)
				{
					if (listItem.name == this.value)
					{
						return listItem.EffectiveDisplayName;
					}
				}
				return this.value;
			}

			// Token: 0x060040C2 RID: 16578 RVA: 0x004B2788 File Offset: 0x004B0988
			[CompilerGenerated]
			private void <_NonMouseModeUpdate>g___ScrollTick|66_0(bool first, ref OpComboBox.<>c__DisplayClass66_0 A_2, ref OpComboBox.<>c__DisplayClass66_1 A_3)
			{

				bool flag = true;
				this._listHover -= Math.Sign(A_3.dir);
				if (this._listHover< 0)

				{
				if (this._listTop > 0)
				{
					this._listTop--;
					this._listHover = 0;
					this._bumpScroll.flash = Mathf.Min(1f, this._bumpScroll.flash + 0.2f);
					this._bumpScroll.sizeBump = Mathf.Min(2.5f, this._bumpScroll.sizeBump + 0.3f);
				}
				else
				{
					flag = false;
					this._listHover = 0;
				}
			}
				if (this._listHover >= this._lblList.Length)

				{
				if (this._listTop < A_2.listSize - this._lblList.Length)
				{
					this._listTop++;
					this._listHover--;
					this._bumpScroll.flash = Mathf.Min(1f, this._bumpScroll.flash + 0.2f);
					this._bumpScroll.sizeBump = Mathf.Min(2.5f, this._bumpScroll.sizeBump + 0.3f);
				}
				else
				{
					flag = false;
					this._listHover--;
				}
			}
				if (flag)
				{
					base.PlaySound(first? SoundID.MENU_First_Scroll_Tick : SoundID.MENU_Scroll_Tick);
		}
	}

	// Token: 0x04003BC0 RID: 15296
	private const float LBLTEXTTRIM = 32f;

	// Token: 0x04003BC1 RID: 15297
	private readonly string DASHES = "------";

	// Token: 0x04003BC2 RID: 15298
	protected ListItem[] _itemList;

	// Token: 0x04003BC3 RID: 15299
	protected List<ListItem> _searchList;

	// Token: 0x04003BC4 RID: 15300
	protected bool _neverOpened;

	// Token: 0x04003BC5 RID: 15301
	protected DyeableRect _rect;

	// Token: 0x04003BC6 RID: 15302
	protected DyeableRect _rectList;

	// Token: 0x04003BC7 RID: 15303
	protected DyeableRect _rectScroll;

	// Token: 0x04003BC8 RID: 15304
	protected FLabel _lblText;

	// Token: 0x04003BC9 RID: 15305
	protected FLabel[] _lblList;

	// Token: 0x04003BCA RID: 15306
	protected FSprite _sprArrow;

	// Token: 0x04003BCB RID: 15307
	public Color colorEdge = MenuColorEffect.rgbMediumGrey;

	// Token: 0x04003BCC RID: 15308
	public Color colorFill = MenuColorEffect.rgbBlack;

	// Token: 0x04003BCE RID: 15310
	public ushort listHeight = 5;

	// Token: 0x04003BCF RID: 15311
	protected bool _mouseDown;

	// Token: 0x04003BD0 RID: 15312
	protected bool _searchMode;

	// Token: 0x04003BD1 RID: 15313
	protected bool _downward = true;

	// Token: 0x04003BD2 RID: 15314
	protected int _dTimer;

	// Token: 0x04003BD3 RID: 15315
	protected int _searchDelay;

	// Token: 0x04003BD4 RID: 15316
	protected int _searchIdle;

	// Token: 0x04003BD5 RID: 15317
	protected int _listTop;

	// Token: 0x04003BD6 RID: 15318
	protected int _listHover = -1;

	// Token: 0x04003BD7 RID: 15319
	protected float _scrollHeldPos;

	// Token: 0x04003BD8 RID: 15320
	protected BumpBehaviour _bumpList;

	// Token: 0x04003BD9 RID: 15321
	protected BumpBehaviour _bumpScroll;

	// Token: 0x04003BDA RID: 15322
	protected string _searchQuery = "";

	// Token: 0x04003BDB RID: 15323
	protected FSprite _searchCursor;

	// Token: 0x04003BDC RID: 15324
	protected int _inputDelay;

	// Token: 0x04003BDD RID: 15325
	private char _lastChar = '\r';

	// Token: 0x04003BDF RID: 15327
	protected GlowGradient _glowFocus;

	// Token: 0x02000C81 RID: 3201
	public class Queue : UIconfig.ConfigQueue
	{
		// Token: 0x06005E79 RID: 24185 RVA: 0x00610186 File Offset: 0x0060E386
		public Queue(Configurable<string> config, object sign = null) : base(config, sign)
		{
			if (config == null || config.info == null || config.info.acceptable == null || !(config.info.acceptable is ConfigAcceptableList<string>))
			{
				throw new ArgumentNullException("To use this constructor, Configurable<string> must have ConfigurableInfo with ConfigAccpetableList.");
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005E7A RID: 24186 RVA: 0x006101C5 File Offset: 0x0060E3C5
		protected override float sizeY
		{
			get
			{
				return 30f;
			}
		}

		// Token: 0x06005E7B RID: 24187 RVA: 0x006101CC File Offset: 0x0060E3CC
		protected override List<UIelement> _InitializeThisQueue(IHoldUIelements holder, float posX, ref float offsetY)
		{
			offsetY += this.sizeY;
			float posY = base.GetPosY(holder, offsetY);
			float width = base.GetWidth(holder, posX, 150f, 50f);
			List<UIelement> list = new List<UIelement>();
			OpComboBox opComboBox = new OpComboBox(this.config as Configurable<string>, new Vector2(posX, posY), width)
			{
				sign = this.sign
			};
			if (this.onChange != null)
			{
				opComboBox.OnChange += this.onChange;
			}
			if (this.onHeld != null)
			{
				opComboBox.OnHeld += this.onHeld;
			}
			if (this.onValueUpdate != null)
			{
				opComboBox.OnValueUpdate += this.onValueUpdate;
			}
			if (this.onValueChanged != null)
			{
				opComboBox.OnValueChanged += this.onValueChanged;
			}
			if (this.onListOpen != null)
			{
				opComboBox.OnListOpen += this.onListOpen;
			}
			if (this.onListClose != null)
			{
				opComboBox.OnListClose += this.onListClose;
			}
			this.mainFocusable = opComboBox;
			list.Add(opComboBox);
			ConfigurableInfo info = this.config.info;
			if (!string.IsNullOrEmpty((info != null) ? info.description : null))
			{
				opComboBox.description = UIQueue.GetFirstSentence(UIQueue.Translate(this.config.info.description));
			}
			if (!string.IsNullOrEmpty(this.config.key))
			{
				OpLabel item = new OpLabel(new Vector2(posX + width + 10f, posY), new Vector2(holder.CanvasSize.x - posX - width - 10f, 30f), UIQueue.Translate(this.config.key), FLabelAlignment.Left, false, null)
				{
					autoWrap = true,
					bumpBehav = opComboBox.bumpBehav,
					description = opComboBox.description
				};
				list.Add(item);
			}
			opComboBox.ShowConfig();
			this.hasInitialized = true;
			return list;
		}

		// Token: 0x04006446 RID: 25670
		public OnSignalHandler onListOpen;

		// Token: 0x04006447 RID: 25671
		public OnSignalHandler onListClose;
	}
		}
	}
	using System;
	using UnityEngine;

	namespace Menu.Remix.MixedUI
	{
		// Token: 0x02000633 RID: 1587
		public abstract class UIconfig : UIfocusable
		{
			// Token: 0x060042AB RID: 17067 RVA: 0x004C34E4 File Offset: 0x004C16E4
			public UIconfig(ConfigurableBase config, Vector2 pos, Vector2 size) : base(pos, size)
			{
				if (config == null)
				{
					throw new ArgumentNullException("config cannot be null. If you want a cosmetic UIconfig, generate cosmetic Configurable with OptionInterface.config.Bind.");
				}
				if (config.BoundUIconfig != null)
				{
					throw new MultiuseConfigurableException(config);
				}
				config.BoundUIconfig = this;
				this.cfgEntry = config;
				this.cosmetic = this.cfgEntry.IsCosmetic;
				this.defaultValue = this.cfgEntry.defaultValue;
				this._value = this.defaultValue;
				this.lastValue = this._value;
			}

			// Token: 0x060042AC RID: 17068 RVA: 0x004C3560 File Offset: 0x004C1760
			public UIconfig(ConfigurableBase config, Vector2 pos, float rad) : base(pos, rad)
			{
				if (config == null)
				{
					throw new ArgumentNullException("config cannot be null. If you want a cosmetic UIconfig, generate cosmetic Configurable with OptionInterface.config.Bind.");
				}
				if (config.BoundUIconfig != null)
				{
					throw new MultiuseConfigurableException(config);
				}
				config.BoundUIconfig = this;
				this.cfgEntry = config;
				this.cosmetic = this.cfgEntry.IsCosmetic;
				this.defaultValue = this.cfgEntry.defaultValue;
				this._value = this.defaultValue;
				this.lastValue = this._value;
			}

			// Token: 0x17000AE1 RID: 2785
			// (get) Token: 0x060042AD RID: 17069 RVA: 0x004C35DA File Offset: 0x004C17DA
			// (set) Token: 0x060042AE RID: 17070 RVA: 0x004C35E2 File Offset: 0x004C17E2
			public string defaultValue { get; protected set; }

			// Token: 0x060042AF RID: 17071 RVA: 0x004C35EB File Offset: 0x004C17EB
			public override void Reset()
			{
				base.Reset();
				this.value = this.defaultValue;
				this.held = false;
			}

			// Token: 0x17000AE2 RID: 2786
			// (get) Token: 0x060042B0 RID: 17072 RVA: 0x004C3606 File Offset: 0x004C1806
			public string Key
			{
				get
				{
					return this.cfgEntry.key;
				}
			}

			// Token: 0x060042B1 RID: 17073 RVA: 0x004C3613 File Offset: 0x004C1813
			public void ForceValue(string newValue)
			{
				this._value = newValue;
			}

			// Token: 0x17000AE3 RID: 2787
			// (get) Token: 0x060042B2 RID: 17074 RVA: 0x004C361C File Offset: 0x004C181C
			// (set) Token: 0x060042B3 RID: 17075 RVA: 0x004C3624 File Offset: 0x004C1824
			public virtual string value
			{
				get
				{
					return this._value;
				}
				set
				{
					if (this._value != value)
					{
						base.FocusMoveDisallow(null);
						string value2 = this._value;
						this._value = value;
						if (!UIelement.ContextWrapped && ConfigContainer.instance != null)
						{
							ConfigContainer.instance.NotifyConfigChange(this, value2, value);
						}
						OnValueChangeHandler onValueUpdate = this.OnValueUpdate;
						if (onValueUpdate != null)
						{
							onValueUpdate(this, value, value2);
						}
						this.Change();
						if (!this.held)
						{
							OnValueChangeHandler onValueChanged = this.OnValueChanged;
							if (onValueChanged != null)
							{
								onValueChanged(this, this._value, this.lastValue);
							}
							this.lastValue = this._value;
						}
					}
				}
			}

			// Token: 0x14000028 RID: 40
			// (add) Token: 0x060042B4 RID: 17076 RVA: 0x004C36BC File Offset: 0x004C18BC
			// (remove) Token: 0x060042B5 RID: 17077 RVA: 0x004C36F4 File Offset: 0x004C18F4
			public event OnValueChangeHandler OnValueUpdate;

			// Token: 0x14000029 RID: 41
			// (add) Token: 0x060042B6 RID: 17078 RVA: 0x004C372C File Offset: 0x004C192C
			// (remove) Token: 0x060042B7 RID: 17079 RVA: 0x004C3764 File Offset: 0x004C1964
			public event OnValueChangeHandler OnValueChanged;

			// Token: 0x060042B8 RID: 17080 RVA: 0x004C379C File Offset: 0x004C199C
			public void ShowConfig()
			{
				this.value = (this.cfgEntry.OI.config.pendingReset ? this.cfgEntry.defaultValue : ValueConverter.ConvertToString(this.cfgEntry.BoxedValue, this.cfgEntry.settingType));
			}

			// Token: 0x060042B9 RID: 17081 RVA: 0x004C37F0 File Offset: 0x004C19F0
			protected internal virtual bool CopyFromClipboard(string value)
			{
				bool result;
				try
				{
					this.value = value;
					this.held = false;
					result = (this.value == value);
				}
				catch
				{
					result = false;
				}
				return result;
			}

			// Token: 0x060042BA RID: 17082 RVA: 0x004C3830 File Offset: 0x004C1A30
			protected internal virtual string CopyToClipboard()
			{
				this.held = false;
				return this.value;
			}

			// Token: 0x060042BB RID: 17083 RVA: 0x004C383F File Offset: 0x004C1A3F
			protected internal override void Deactivate()
			{
				this.held = false;
				base.Deactivate();
			}

			// Token: 0x060042BC RID: 17084 RVA: 0x004C384E File Offset: 0x004C1A4E
			protected internal override void Unload()
			{
				base.Unload();
				this.cfgEntry.BoundUIconfig = null;
			}

			// Token: 0x17000AE4 RID: 2788
			// (get) Token: 0x060042BD RID: 17085 RVA: 0x004C3862 File Offset: 0x004C1A62
			// (set) Token: 0x060042BE RID: 17086 RVA: 0x004C386C File Offset: 0x004C1A6C
			protected internal override bool held
			{
				get
				{
					return base.held;
				}
				set
				{
					base.held = value;
					if (!value && base.Focused && this.lastValue != this._value)
					{
						OnValueChangeHandler onValueChanged = this.OnValueChanged;
						if (onValueChanged != null)
						{
							onValueChanged(this, this._value, this.lastValue);
						}
					}
					this.lastValue = this._value;
				}
			}

			// Token: 0x060042BF RID: 17087 RVA: 0x004C38C8 File Offset: 0x004C1AC8
			internal void _UndoCallChanges()
			{
				OnValueChangeHandler onValueUpdate = this.OnValueUpdate;
				if (onValueUpdate != null)
				{
					onValueUpdate(this, this._value, this.lastValue);
				}
				OnValueChangeHandler onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged(this, this._value, this.lastValue);
				}
				this.lastValue = this._value;
			}

			// Token: 0x04003D08 RID: 15624
			internal const string errorNull = "config cannot be null. If you want a cosmetic UIconfig, generate cosmetic Configurable with OptionInterface.config.Bind.";

			// Token: 0x04003D09 RID: 15625
			public readonly ConfigurableBase cfgEntry;

			// Token: 0x04003D0B RID: 15627
			public readonly bool cosmetic;

			// Token: 0x04003D0E RID: 15630
			protected string lastValue;

			// Token: 0x04003D0F RID: 15631
			protected string _value;

			// Token: 0x02000CA5 RID: 3237
			public abstract class ConfigQueue : UIfocusable.FocusableQueue
			{
				// Token: 0x06005EC1 RID: 24257 RVA: 0x00612A05 File Offset: 0x00610C05
				protected ConfigQueue(ConfigurableBase config, object sign = null) : base(sign)
				{
					this.config = config;
				}

				// Token: 0x040064B7 RID: 25783
				public readonly ConfigurableBase config;

				// Token: 0x040064B8 RID: 25784
				public OnValueChangeHandler onValueUpdate;

				// Token: 0x040064B9 RID: 25785
				public OnValueChangeHandler onValueChanged;
			}
		}
	}*/
    #endregion

    // 复制
    #region ParticleEffect

    /*using HUD;
using RWCustom;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnderPearl
{
	public class ParticleEffect
	{
		private class Particle
		{
			public FContainer? container;
			public Vector2 velocity;
			public Vector2 worldPosition;
			public LightSource? lightSource;
			public float timer = 0f;
			public bool isActive = true;
		}

		private List<Particle> particles = new List<Particle>();
		private FContainer masterContainer;

		public float pixelSize = 1.5f;
		private const float maxLifetime = 90f; // 90帧 
		private float currentLifetime = 0f;
		public bool Destroyed = false;

		private Vector2 targetWorldPos;
		private bool reverse;
		private Room room;

		public ParticleEffect(Room room, Vector2 targetWorldPos, bool reverse)
		{
			this.targetWorldPos = targetWorldPos;
			this.reverse = reverse;
			this.room = room;

			try
			{
				// 创建主容器
				masterContainer = new FContainer();
				Futile.stage.AddChild(masterContainer);

				int particleCount = UnityEngine.Random.Range(15, 30);

				for (int i = 0; i < particleCount; i++)
				{
					CreateParticle();
				}
			}
			catch (System.Exception e)
			{
				Log.LogError($"粒子效果初始化失败: {e.Message}");
				Destroy();
				throw;
			}
		}

		private void CreateParticle()
		{
			Particle particle = new Particle();
			particle.container = new FContainer();
			masterContainer.AddChild(particle.container);
			particle.container.alpha = 0f; // 初始透明
			particle.container.isVisible = true;

			// 生成随机粒子形状
			GenerateRandomParticle(particle.container);
			particle.container.rotation = 90 * UnityEngine.Random.Range(0, 4);

			// 初始化位置和速度
			InitializeParticlePosition(particle);

			particles.Add(particle);
		}

		private void InitializeParticlePosition(Particle particle)
		{
			Vector2 cameraPos = room.game.cameras[0].pos;

			float TWO_PT = Mathf.PI * 2f;
			if (reverse)
			{
				// 反向粒子：从远处向目标汇聚
				float angle = UnityEngine.Random.Range(0f, TWO_PT);
				float radius = Mathf.Sqrt(UnityEngine.Random.Range(30f * 30f, 300f * 300f));

				// 世界坐标位置
				particle.worldPosition = targetWorldPos + new Vector2(
					Mathf.Cos(angle),
					Mathf.Sin(angle)
				) * radius;

				// 速度方向指向目标
				particle.velocity = (targetWorldPos - particle.worldPosition).normalized *
								   UnityEngine.Random.Range(radius / 100, radius / 50); // 初始较快速度
			}
			else
			{
				// 正向粒子：从目标向外扩散
				float angle = UnityEngine.Random.Range(0f, TWO_PT);
				float radius = UnityEngine.Random.Range(0f, 30f);

				// 世界坐标位置
				particle.worldPosition = targetWorldPos + new Vector2(
					Mathf.Cos(angle),
					Mathf.Sin(angle)
				) * radius;

				// 速度方向向外扩散
				particle.velocity = (particle.worldPosition - targetWorldPos).normalized *
								   UnityEngine.Random.Range(1f, 5f); // 初始较快速度
			}

			// 设置屏幕位置
			UpdateScreenPosition(particle, cameraPos);
		}

		public void Update()
		{
			if (Destroyed) return;

			Vector2 cameraPos = room.game.cameras[0].pos;
			Rect cameraRect = new Rect(
				cameraPos.x - 1000f,
				cameraPos.y - 1000f,
				Custom.rainWorld.options.ScreenSize.x + 2000f,
				Custom.rainWorld.options.ScreenSize.y + 2000f
			);

			bool anyActive = false;
			currentLifetime += 1f;

			for (int i = particles.Count - 1; i >= 0; i--)
			{
				Particle particle = particles[i];

				if (!particle.isActive) continue;

				// 更新粒子计时器
				particle.timer += 1f;

				// 屏幕外检测
				if (!cameraRect.Contains(particle.worldPosition))
				{
					RemoveParticle(particle);
					continue;
				}

				// 更新位置和速度
				UpdateParticlePosition(particle);

				// 更新屏幕位置
				UpdateScreenPosition(particle, cameraPos);

				// 更新光源
				UpdateLightSource(particle);

				// 更新透明度
				UpdateParticleAlpha(particle);

				anyActive = true;
			}

			// 检查是否所有粒子都已销毁
			if (!anyActive || currentLifetime >= maxLifetime)
			{
				Destroy();
			}
		}

		private void UpdateParticlePosition(Particle particle)
		{
			// 粒子生命周期阶段划分（基于计时器）
			if (particle.timer < 30f)
			{
				// 第一阶段：0-30帧 - 快速移动
				// 保持初始速度
			}
			else if (particle.timer < 40f)
			{
				// 第二阶段：30-40帧 - 减速
				float slowdownFactor = 1f - (particle.timer - 30f) / 10f; // 从1到0线性减速
				particle.velocity *= slowdownFactor;
			}
			else
			{
				// 第三阶段：40+帧 - 停止
				particle.velocity = Vector2.zero;
			}

			// 更新世界位置
			particle.worldPosition += particle.velocity;
		}

		private void UpdateScreenPosition(Particle particle, Vector2 cameraPos)
		{
			// 世界坐标转屏幕坐标
			Vector2 screenPos = new Vector2(
				particle.worldPosition.x - cameraPos.x,
				particle.worldPosition.y - cameraPos.y
			);

			particle.container.SetPosition(screenPos);
		}

		private void UpdateLightSource(Particle particle)
		{
			Vector2 cameraPos = room.game.cameras[0].pos;
			Vector2 screenPos = new Vector2(
				particle.worldPosition.x - cameraPos.x,
				particle.worldPosition.y - cameraPos.y
			);

			if (particle.lightSource == null)
			{
				// 使用正确的世界坐标创建光源
				particle.lightSource = new LightSource(
					screenPos,
					false,
					new Color(209f / 255f, 69f / 255f, 247f / 255f),
					null
				);

				if (room != null)
				{
					room.AddObject(particle.lightSource);
				}
			}
			else
			{
				// 更新光源位置（使用世界坐标）
				particle.lightSource.setPos = screenPos;
				particle.lightSource.requireUpKeep = true;

				// 设置光源半径（根据生命周期调整）
				float radius = 0f;
				if (particle.timer < 30f)
				{
					// 0-30帧：光源半径逐渐增大
					radius = 50f * (particle.timer / 30f);
				}
				else if (particle.timer < 40f)
				{
					// 30-40帧：保持最大半径
					radius = 50f;
				}
				else
				{
					// 40+帧：光源半径随透明度减小
					radius = 50f * particle.container.alpha;
				}

				particle.lightSource.setRad = radius;
				particle.lightSource.stayAlive = true;

				// 更新光源透明度
				particle.lightSource.setAlpha = particle.container.alpha;

				// 检查光源是否需要销毁
				if (particle.lightSource.slatedForDeletetion || particle.lightSource.room != room)
				{
					particle.lightSource.Destroy();
					particle.lightSource = null;
				}
			}
		}

		private void UpdateParticleAlpha(Particle particle)
		{
			if (particle.timer < 10f)
			{
				// 0-10帧：快速显示（淡入）
				particle.container.alpha = particle.timer / 10f;
			}
			else if (particle.timer < 40f)
			{
				// 10-40帧：保持完全可见
				particle.container.alpha = 1f;
			}
			else
			{
				// 40+帧：缓慢消失（淡出）
				float fadePhase = (particle.timer - 40f) / 40f;
				particle.container.alpha = Mathf.Max(0f, 1f - fadePhase);

				// 完全透明后标记为不活跃
				if (particle.container.alpha <= 0f)
				{
					particle.isActive = false;
				}
			}
		}

		private void RemoveParticle(Particle particle)
		{
			if (particle.container != null)
			{
				masterContainer.RemoveChild(particle.container);
				particle.container = null;
			}

			if (particle.lightSource != null)
			{
				particle.lightSource.Destroy();
				particle.lightSource = null;
			}

			particles.Remove(particle);
		}

		public void Destroy()
		{
			if (Destroyed) return;

			Destroyed = true;

			// 销毁所有粒子
			for (int i = particles.Count - 1; i >= 0; i--)
			{
				RemoveParticle(particles[i]);
			}

			// 销毁主容器
			if (masterContainer != null)
			{
				Futile.stage.RemoveChild(masterContainer);
				masterContainer = null;
			}
		}

		private void GenerateRandomParticle(FContainer container)
		{
			int shapeType = UnityEngine.Random.Range(0, 6);

			switch (shapeType)
			{
				case 0:
					CreatePixel(container, Vector2.zero);
					break;

				case 1:
					CreateShape1(container);
					break;

				case 2:
					CreateShape2(container);
					break;

				case 3:
					CreateShape3(container);
					break;

				default:
					CreatePixel(container, Vector2.zero);
					break;
			}
		}

		private void CreatePixel(FContainer container, Vector2 position)
		{
			FSprite pixel = new FSprite("pixel")
			{
				width = pixelSize,
				height = pixelSize,
				color = new Color(209f / 255f, 69f / 255f, 247f / 255f, 1f),
				x = position.x,
				y = position.y
			};
			container.AddChild(pixel);
		}

		private void CreateShape1(FContainer container)
		{
			Vector2[] positions = {
				new Vector2(0, 0),
				new Vector2(pixelSize, 0),
				new Vector2(pixelSize, pixelSize),
				new Vector2(pixelSize, pixelSize * 2),
				new Vector2(0, pixelSize * 2),
				new Vector2(-pixelSize, pixelSize * 2),
				new Vector2(-pixelSize * 2, pixelSize * 2),
				new Vector2(-pixelSize * 2, pixelSize),
				new Vector2(-pixelSize * 2, 0),
				new Vector2(-pixelSize * 2, -pixelSize),
				new Vector2(-pixelSize * 2, -pixelSize * 2),
				new Vector2(-pixelSize, -pixelSize * 2),
				new Vector2(0, -pixelSize * 2),
				new Vector2(pixelSize, -pixelSize * 2)
			};

			foreach (Vector2 pos in positions)
			{
				CreatePixel(container, pos);
			}
		}

		private void CreateShape2(FContainer container)
		{
			Vector2[] positions = {
				new Vector2(0, 0),
				new Vector2(pixelSize * 2, 0),
				new Vector2(-pixelSize * 2, 0),
				new Vector2(pixelSize, pixelSize),
				new Vector2(-pixelSize, pixelSize),
				new Vector2(pixelSize * 2, pixelSize * 2),
				new Vector2(-pixelSize * 2, pixelSize * 2),
				new Vector2(0, pixelSize * 2),
				new Vector2(pixelSize, -pixelSize),
				new Vector2(-pixelSize, -pixelSize),
				new Vector2(pixelSize * 2, -pixelSize * 2),
				new Vector2(-pixelSize * 2, -pixelSize * 2),
				new Vector2(0, -pixelSize * 2)
			};

			foreach (Vector2 pos in positions)
			{
				CreatePixel(container, pos);
			}
		}

		private void CreateShape3(FContainer container)
		{
			Vector2[] positions = {
				// 中心点
				new Vector2(0, 0),
		
				// 45° 对角线散射
				new Vector2(pixelSize, pixelSize),
				new Vector2(-pixelSize, -pixelSize),
				new Vector2(pixelSize, -pixelSize),
				new Vector2(-pixelSize, pixelSize),
		
				// 横向纵向扩展
				new Vector2(pixelSize * 2, 0),
				new Vector2(-pixelSize * 2, 0),
				new Vector2(0, pixelSize * 2),
				new Vector2(0, -pixelSize * 2),
		
				// 外层对角线大范围
				*//*new Vector2(pixelSize * 3, pixelSize * 3),
				new Vector2(-pixelSize * 3, -pixelSize * 3),
				new Vector2(pixelSize * 3, -pixelSize * 3),
				new Vector2(-pixelSize * 3, pixelSize * 3)*//*
			};

			foreach (Vector2 pos in positions)
			{
				CreatePixel(container, pos);
			}
		}


	}
}

*/

    #endregion
    #region IL

    //private static void Player_GrabUpdate(ILContext il)
    //{
    //	Logs.Write("[Player_GrabUpdate] Patch entered");

    //	ILCursor c = new ILCursor(il);

    //	Dictionary<int, bool> logs = new();
    //	for (int i = 0; i < 15; i++)
    //	{
    //		logs.Add(i, false);
    //	}
    //	int logIndex = 0;

    //	logIndex = 1;
    //	// ============================================================
    //	// 问题1: IL_0451 附近
    //	// 原版: if (objectInStomach == null || CanPutSpearToBack || CanPutSlugToBack)
    //	// 改为: if (stomachList.Count < maxCapacity || CanPutSpearToBack || CanPutSlugToBack)
    //	// 用途: 判断是否能拿取新物品（胃有空位 或 能放背上）
    //	// ============================================================
    //	/*if (c.TryGotoNext(MoveType.Before,
    //		i => i.MatchLdarg(0),
    //		i => i.MatchLdfld<Player>("objectInStomach"),
    //		i => i.MatchBrfalse(out _),
    //		i => i.MatchLdarg(0),
    //		i => i.MatchCall<Player>("get_CanPutSpearToBack")
    //		))
    //	{
    //		LogsWrite(c, logs, logIndex);

    //		c.Remove();
    //		c.Remove();
    //		//c.Remove();

    //		// 插入: 检查胃是否未满
    //		//原逻辑 objectInStomach == null → brfalse 跳转(满足条件)

    //		c.Emit(OpCodes.Ldarg, 0);
    //		c.EmitDelegate<Func<Player, bool>>(player =>
    //		{
    //			Log.LogInfo($"[{logIndex}]");

    //			player.GetPlayerVar(out var pv);
    //			var stomachData = pv.stomachData;
    //			return stomachData.IsFull;
    //		});
    //		//c.Emit(OpCodes.Brfalse,  跳转到 IL_046c 的标签 );
    //	}
    //	else
    //	{
    //		Logs.Write($"[Player_GrabUpdate] [logIndex:{logIndex}] not found");
    //	}*/

    //	// 找到跳转目标，创建标签
    //	//ILLabel? targetLabel = null;

    // //         logIndex = 3;
    //	//if (c.TryGotoNext(MoveType.Before,
    //	//	i => i.MatchLdarg(0),
    //	//	i => i.MatchLdfld<Player>("objectInStomach"),//objectInStomach spearOnBack
    //	//	i => i.MatchBrtrue(out _),
    //	//	i => i.MatchLdarg(0),
    //	//	i => i.MatchCall<Player>("get_isGourmand"),
    //	//	i => i.MatchBrtrue(out _)
    //	//	//i => i.Match(OpCodes.Brtrue) || i.Match(OpCodes.Brfalse)
    //	//	))
    //	//{
    //	//	LogsWrite(c, logs, logIndex);

    //	//	/*c.Emit(OpCodes.Ldarg, 0);

    //	//	c.EmitDelegate<Action<Player>>(player =>
    //	//	{
    //	//		if (!logs[logIndex] || Input.GetKey("c"))
    //	//		{
    //	//			logs[logIndex] = true;

    //	//			Log.LogInfo($"[{logIndex}] {logIndex}");

    //	//			Logs.Write($"[Player_GrabUpdate]-[{logIndex}] {logIndex}");
    //	//		}
    //	//	});*/

    //	//	c.Remove();
    //	//	c.Remove();

    //	//	c.Emit(OpCodes.Ldarg, 0);

    //	//	c.EmitDelegate<Func<Player, bool>>(player =>
    //	//	{
    //	//		//Log.LogInfo($"[logIndex:{logIndex}]");

    //	//		player.GetPlayerVar(out var pv);
    //	//		var stomachData = pv.stomachData;

    //	//		int canBeSwallow = BeSwallowed(player);

    //	//		if (canBeSwallow != -1)// 可以被吞咽
    //	//		{
    //	//			Log.LogInfo($"可以被吞咽");
    //	//			return false;// 不要反刍
    //	//		}
    //	//		Log.LogInfo($"不可以被吞咽 !IsEmpty[{!stomachData.IsEmpty}]");
    //	//		return !stomachData.IsEmpty;
    //	//	});
    //	//}
    //	//else
    //	//{
    //	//	Logs.Write($"[Player_GrabUpdate] [logIndex:{logIndex}] not found");
    //	//}



    //	logIndex = 5;
    //	if (c.TryGotoNext(MoveType.Before,
    //		i => i.MatchLdarg(0),
    //		i => i.MatchLdfld<Player>("objectInStomach"),
    //		i => i.MatchBrtrue(out _),// 出去if
    //		i => i.MatchLdarg(0),
    //		i => i.MatchLdfld<Player>("swallowAndRegurgitateCounter"),
    //		i => i.MatchLdcI4(90),
    //		i => i.MatchBle(out _)
    //		))
    //	{
    //		LogsWrite(c, logs, logIndex);

    //		// 手动获取 brtrue 指令（当前 cursor 在 ldarg.0，brtrue 是第3条，index+2）
    //		/*var brtrueInst = c.Instrs[c.Index + 2];

    //		// 获取跳转目标
    //		var targetInst = (Instruction)brtrueInst.Operand;

    //		// 创建标签
    //		targetLabel = il.DefineLabel();

    //		// 在目标位置打标签（需要另一个 cursor）
    //		var markCursor = new ILCursor(il);
    //		markCursor.Goto(targetInst, MoveType.Before);
    //		markCursor.MarkLabel(targetLabel);*/

    //		c.Remove();
    //		c.Remove();
    //		//c.Remove();

    //		c.Emit(OpCodes.Ldarg, 0);

    //		c.EmitDelegate<Func<Player, bool>>(player =>
    //		{
    //			//Log.LogInfo($"[logIndex:{logIndex}]");

    //			player.GetPlayerVar(out var pv);
    //			var stomachData = pv.stomachData;

    //			int canBeSwallow = BeSwallowed(player);

    //			if (canBeSwallow == -1)// 不可以被吞咽
    //			{
    //				Log.LogInfo($"不可以被吞咽");
    //				return true;// 不要吞咽
    //			}
    //			Log.LogInfo($"可以被吞咽 IsFull[{stomachData.IsFull}]");
    //			return stomachData.IsFull;// 出去if
    //		});
    //		// 用标签
    //		//c.Emit(OpCodes.Brtrue, targetLabel);
    //	}
    //	else
    //	{
    //		Logs.Write($"[Player_GrabUpdate] [logIndex:{logIndex}] not found");
    //	}



    //	Logs.Write("[Player_GrabUpdate] Patch entered_");

    //}

    //public static void LogsWrite(ILCursor c, Dictionary<int, bool> logs, int logIndex)
    //{
    //	Logs.Write($"[Player_GrabUpdate]-[{logIndex}] found");

    //	/*c.Emit(OpCodes.Ldarg, 0);

    //	c.EmitDelegate<Action<Player>>(player =>
    //	{
    //		if (!logs[logIndex] || Input.GetKey("c"))
    //		{
    //			logs[logIndex] = true;

    //			Log.LogInfo($"[{logIndex}] {logIndex}");

    //			Logs.Write($"[Player_GrabUpdate]-[{logIndex}] {logIndex}");
    //		}
    //	});*/
    //}

    #endregion
    #region IL2
    /*if (c.TryGotoNext(MoveType.Before,
		i => i.MatchLdarg(0),
		i => i.MatchLdfld<Player>("objectInStomach"),
		i => i.MatchBrfalse(out ILLabel label)))
	{
		c.Remove();
		c.Remove();

		c.Emit(OpCodes.Ldarg, 0);
		c.EmitDelegate<Func<Player, bool>>(player =>
		{
			*//*if (!logs[1] || Input.GetKey("c"))
			{
				logs[1] = true;

				Log.LogInfo($"1");
			}*//*

			Log.LogInfo($"1");

			player.GetPlayerVar(out var pv);
			return pv.stomachData.IsFull;
		});
	}*/

    //// ============================================================
    //// 问题1: IL_0451 附近
    //// 原版: if (objectInStomach == null || CanPutSpearToBack || CanPutSlugToBack)
    //// 改为: if (stomachList.Count < maxCapacity || CanPutSpearToBack || CanPutSlugToBack)
    //// 用途: 判断是否能拿取新物品（胃有空位 或 能放背上）
    //// ============================================================

    //if (c.TryGotoNext(MoveType.Before,
    //	i => i.MatchLdarg(0),
    //	i => i.MatchLdfld<Player>("objectInStomach"),
    //	i => i.MatchBrfalse(out _)
    //	))
    //{
    //	// 移除 ldarg.0 + ldfld
    //	c.Remove();
    //	c.Remove();
    //	//c.Remove();

    //	// 插入: 检查胃是否未满
    //	// 如果胃未满 → brfalse 跳转（原逻辑是 objectInStomach == null 时跳转）
    //	c.Emit(OpCodes.Ldarg, 0);
    //	//c.Emit(OpCodes.Call, typeof(StomachUtils).GetMethod("IsStomachNotFull"));
    //	c.EmitDelegate<Func<Player, bool>>(player =>
    //	{
    //		Log.LogInfo($"1");

    //		player.GetPlayerVar(out var pv);
    //		var stomachData = pv.stomachData;
    //		return stomachData.IsFull;
    //	});
    //	//c.Emit(OpCodes.Brfalse, /* 跳转到 IL_046c 的标签 */);
    //}


    //// ============================================================
    //// 问题2: IL_0639-IL_0646 附近
    //// 原版: if (num8 > -1 || objectInStomach != null || isGourmand)
    //// 改为: if (num8 > -1 || stomachList.Count > 0 || isGourmand)
    //// 用途: 判断是否可以进行吞咽/反刍操作
    //// ============================================================

    //if (c.TryGotoNext(MoveType.Before,
    //	i => i.MatchLdarg(0),
    //	i => i.MatchLdfld("Player", "objectInStomach"),
    //	i => i.MatchBrtrue(out _)))
    //{
    //	c.Remove();
    //	c.Remove();
    //	//c.Remove();

    //	// 插入: 检查胃是否有物品
    //	c.Emit(OpCodes.Ldarg, 0);
    //	//c.Emit(OpCodes.Call, typeof(StomachUtils).GetMethod("HasItemsInStomach"));
    //	c.EmitDelegate<Func<Player, bool>>(player =>
    //	{
    //		Log.LogInfo($"2");

    //		player.GetPlayerVar(out var pv);
    //		var stomachData = pv.stomachData;
    //		return !stomachData.IsEmpty;
    //	});
    //	//c.Emit(OpCodes.Brtrue, /* 跳转到原 brtrue 的目标标签 */);
    //}

    //// ============================================================
    //// 问题3: IL_18cf-IL_18d5
    //// 原版: if (objectInStomach != null || isGourmand || (MSC && SlugCatClass == Spear))
    //// 改为: if (stomachList.Count > 0 || isGourmand || (MSC && SlugCatClass == Spear))
    //// 用途: 反刍条件判定 (swallowAndRegurgitateCounter > 110)
    //// ============================================================

    //if (c.TryGotoNext(MoveType.Before,
    //	i => i.MatchLdarg(0),
    //	i => i.MatchLdfld("Player", "objectInStomach"),
    //	i => i.MatchBrtrue(out _)))
    //{
    //	c.Remove();
    //	c.Remove();
    //	//c.Remove();

    //	c.Emit(OpCodes.Ldarg, 0);
    //	//c.Emit(OpCodes.Call, typeof(StomachUtils).GetMethod("HasItemsInStomach"));
    //	c.EmitDelegate<Func<Player, bool>>(player =>
    //	{
    //		Log.LogInfo($"3");

    //		return false;

    //		/*player.GetPlayerVar(out var pv);
    //		var stomachData = pv.stomachData;

    //		int canBeSwallow = BeSwallowed(player);

    //		if (canBeSwallow != -1)
    //		{
    //			return false;
    //		}

    //		return !stomachData.IsEmpty;*/
    //	});
    //	//c.Emit(OpCodes.Brtrue, /* 跳转到原 brtrue 的目标标签 (IL_18fe) */);
    //}

    //// ============================================================
    //// 问题4: IL_1916-IL_191c
    //// 原版: if (isGourmand && objectInStomach == null)
    //// 改为: if (isGourmand && stomachList.Count == 0)
    //// 用途: Gourmand 特殊反刍逻辑
    //// ============================================================

    //if (c.TryGotoNext(MoveType.Before,
    //	i => i.MatchLdarg(0),
    //	i => i.MatchLdfld("Player", "objectInStomach"),
    //	i => i.MatchBrtrue(out _)))  // 注意这里 brtrue 表示 objectInStomach != null 时跳转
    //{
    //	c.Remove();
    //	c.Remove();
    //	//c.Remove();

    //	// 插入: 检查胃是否为空 (Count == 0)
    //	c.Emit(OpCodes.Ldarg, 0);
    //	//c.Emit(OpCodes.Call, typeof(StomachUtils).GetMethod("IsStomachEmpty"));
    //	c.EmitDelegate<Func<Player, bool>>(player =>
    //	{
    //		Log.LogInfo($"4");

    //		player.GetPlayerVar(out var pv);
    //		var stomachData = pv.stomachData;
    //		return !stomachData.IsEmpty;
    //	});
    //	//c.Emit(OpCodes.Brtrue, /* 跳转到原 brtrue 的目标标签 (IL_1921) */);
    //}

    //// ============================================================
    //// 问题5: IL_19be-IL_19c4
    //// 原版: else if (objectInStomach == null && swallowAndRegurgitateCounter > 90)
    //// 改为: else if (stomachList.Count == 0 && swallowAndRegurgitateCounter > 90)
    //// 用途: 吞咽条件 (胃为空 + 计数器 > 90)
    //// ============================================================

    //if (c.TryGotoNext(MoveType.Before,
    //	i => i.MatchLdarg(0),
    //	i => i.MatchLdfld("Player", "objectInStomach"),
    //	i => i.MatchBrtrue(out _)))  // objectInStomach != null 时跳转到 IL_1af9 (跳过吞咽)
    //{
    //	c.Remove();
    //	c.Remove();
    //	//c.Remove();

    //	// 插入: 检查胃是否为空 (Count == 0)
    //	c.Emit(OpCodes.Ldarg, 0);
    //	//c.Emit(OpCodes.Call, typeof(StomachUtils).GetMethod("IsStomachEmpty"));
    //	c.EmitDelegate<Func<Player, bool>>(player =>
    //	{
    //		Log.LogInfo($"5");

    //		player.GetPlayerVar(out var pv);
    //		var stomachData = pv.stomachData;

    //		if (Input.GetKey("c"))//
    //		{
    //			foreach (var instruct in il.Instrs)
    //			{
    //				Debug.Log(instruct.ToString());
    //			}
    //		}


    //		return stomachData.IsFull;
    //	});
    //	//c.Emit(OpCodes.Brtrue, /* 跳转到原 brtrue 的目标标签 (IL_1af9，跳过吞咽) */);
    //}


    //Debug.Log("===After");

    //c.Emit(OpCodes.Ldarg_0);
    //c.EmitDelegate<Action<Player>>((player) =>
    //{
    //	if (Input.GetKey("c"))
    //	{
    //		foreach (var instruct in il.Instrs)
    //		{
    //			Log.LogInfo(instruct.ToString());
    //		}
    //	}
    //	Log.LogInfo($"10");
    //});

    /*// 用于标记我们处理到了哪一处
	int patchCount = 0;

	while (patchCount < 5)
	{
		patchCount++;

		// 遍历所有 "ldarg.0 + ldfld objectInStomach" 模式

		if (c.TryGotoNext(MoveType.Before,
			i => i.MatchLdarg(0),
			i => i.MatchLdfld<Player>("objectInStomach"),
			i => i.MatchBrfalse(out _)
		))
		{
			// 移除原来的 ldarg.0 和 ldfld
			c.Remove();
			c.Remove();

			// 原代码: if (objectInStomach == null) -> 跳转
			// 需要替换为: if (IsStomachEmpty(player)) -> 跳转

			// 插入: if (IsStomachEmpty(player)) brfalse ...
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Func<Player, bool>>(player =>
			{
				player.GetPlayerVar(out var pv);
				var stomachData = pv.stomachData;
				return stomachData.IsFull;
			});
			// 注意：原 brfalse 指令还在后面，会使用栈上的 bool 值
		}


		else if (c.TryGotoNext(MoveType.Before,
			i => i.MatchLdarg(0),
			i => i.MatchLdfld<Player>("objectInStomach"),
			i => i.MatchBrtrue(out _)
		))
		{
			// 移除原来的 ldarg.0 和 ldfld
			c.Remove();
			c.Remove();

			// 原代码: if (objectInStomach != null) -> 跳转
			// 需要替换为: if (!IsStomachEmpty(player)) -> 跳转

			// 插入: if (IsStomachEmpty(player)) brtrue ...
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Func<Player, bool>>(player =>
			{
				player.GetPlayerVar(out var pv);
				var stomachData = pv.stomachData;
				return stomachData.IsFull;
			});
			// 注意：原 brtrue 指令还在后面，会使用栈上的 bool 值
		}


	}*/
    #endregion
    #region PlayerVar

    /*
	 using Kittehface.Framework20;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	namespace ExtensionLib
	{
		public class PlayerVar
		{
			#region PlayerRef
			[JsonIgnore]
			private WeakReference<Player?> _playerRef;

			[JsonIgnore]
			public WeakReference<Player?> PlayerRef => _playerRef;


			// 供外部重新绑定 Player 引用
			public void SetPlayerRef(Player player) { _playerRef = new WeakReference<Player?>(player); }
			#endregion

			public StomachData stomachData;

			[JsonIgnore]
			public WorldCoordinate coord;

			public class StomachData
			{
				[JsonIgnore]
				public PlayerVar Owner;

				[JsonIgnore]
				public List<AbstractPhysicalObject> historyInStomach = new();  // 历史栈（不含当前）

				[JsonProperty("capacity")]
				public int capacity = 5;                               // 容量限制

				[JsonProperty("historyInStomach")]
				public List<string> _serializedHistory = new();


				public StomachData(PlayerVar owner)
				{
					Owner = owner;
				}
				public AbstractPhysicalObject? Current      // 指向 player.objectInStomach
				{
					get
					{
						Owner._playerRef.TryGetTarget(out Player? player);
						if (player == null)
						{
							throw new NullReferenceException("player == null".MessageLog());
						}
						return player.objectInStomach;
					}
					set
					{
						Owner._playerRef.TryGetTarget(out Player? player);
						if (player == null)
						{
							throw new NullReferenceException("player == null".MessageLog());
						}
						player.objectInStomach = value;
					}
				}
				public int HistoryCount => historyInStomach.Count;
				public int TotalCount => (Current != null ? 1 : 0) + HistoryCount;
				public bool IsEmpty => Current == null && HistoryCount == 0;
				public bool IsFull => TotalCount >= capacity;
				public int RemainingSpace => capacity - TotalCount;
				public int Capacity { get => capacity; set => capacity = Math.Max(0, value); }

				// 历史栈操作
				public void Push(AbstractPhysicalObject obj) => historyInStomach.Add(obj);
				public AbstractPhysicalObject? Pop()
				{
					if (HistoryCount == 0) return null;
					var top = historyInStomach[HistoryCount - 1];
					historyInStomach.RemoveAt(HistoryCount - 1);
					return top;
				}
				public void ClearHistory() => historyInStomach.Clear();

				// 吞入
				public bool Swallow(AbstractPhysicalObject obj)
				{
					if (IsFull) return false;

					// 当前变历史
					if (Current != null)
						Push(Current);

					// 设置新当前
					Current = obj;
					return true;
				}
				// 吐出/消化（移除当前，从历史补位）
				public AbstractPhysicalObject? PopCurrent()
				{
					var removed = Current;

					// 从历史取出补位
					if (HistoryCount > 0)
					{
						Current = Pop();
					}
					else
					{
						Current = null;
					}

					return removed;
				}

				// 清空全部
				public void ClearAll()
				{
					ClearHistory();
					Current = null;
				}

				// 获取完整内容（用于遍历/显示）
				public List<AbstractPhysicalObject> GetAllContents()
				{
					var result = historyInStomach;  // 历史（从底到顶）
					if (Current != null)
						result.Add(Current);    // 当前（栈顶）
					return result;
				}
			}



			// 反序列化的无参构造函数
			public PlayerVar()
			{
				_playerRef = new WeakReference<Player?>(null);

				stomachData = new StomachData(this);
				coord = new WorldCoordinate();
			}
			public PlayerVar(Player player)
			{
				_playerRef = new WeakReference<Player?>(player);

				stomachData = new StomachData(this);
				coord = player.coord;
			}



			#region swallowedObjects
			//public int StorageCapacity = 1;
			//[JsonIgnore]
			//public List<AbstractPhysicalObject> objectsInStomach = new();//胃部存储列表
			//public List<string> swallowedObjects = new();
			//[JsonIgnore]
			//public WorldCoordinate coord;

			//// 获取胃部容量
			//public int GetStomachCapacity(Player player)
			//{
			//	int Capacity = StorageCapacity;

			//	/*if (MyOptions.Instance?.StomachCapacity != null)
			//          {
			//              Capacity = MyOptions.Instance.StomachCapacity.Value;
			//          }
	//	/*if (ModManager.MSC && player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
	//          {
	//              Capacity += 2;
	//          }

	//	return Capacity;
	//}

	//// 检查是否有空间
	//public bool HasSpace(Player player)
	//{
	//	return objectsInStomach.Count < GetStomachCapacity(player);
	//}
	#endregion

	#region Save/Load

	[OnSerializing]// 保存时自动调用
	internal void OnSerializing(StreamingContext context)
	{
		Save();
	}
	public void Save()
	{
		_playerRef.TryGetTarget(out Player? player);

		stomachData._serializedHistory.Clear();

		foreach (var obj in stomachData.historyInStomach)
		{
			string objString = Helper.ObjectToString(obj, player != null ? player.coord : coord, true);
			stomachData._serializedHistory.Add(objString);
		}
		Log.LogDebug($"Save: 保存了 {stomachData.historyInStomach.Count} 个对象");
	}
	public void Malnourished_Save()
	{
		Save();

		stomachData.historyInStomach.Clear();
	}

	[OnDeserialized]// 加载时自动调用
	internal void OnDeserialized(StreamingContext context)
	{
		Load();
	}
	public void Load()
	{
		/*_playerRef.TryGetTarget(out Player? player);
		objectsInStomach.Clear();

		foreach (string objString in _savedSwallowedObjects)
		{
			AbstractPhysicalObject obj = Helper.StringToObject(objString, player != null ? player.coord : coord);
			if (obj != null)
			{
				objectsInStomach.Add(obj);
			}
		}
	}
			#endregion






		}
	}

	*/

    #endregion
    #region swallowedObjects
    //public int StorageCapacity = 1;
    //[JsonIgnore]
    //public List<AbstractPhysicalObject> objectsInStomach = new();//胃部存储列表
    //public List<string> swallowedObjects = new();
    //[JsonIgnore]
    //public WorldCoordinate coord;

    //// 获取胃部容量
    //public int GetStomachCapacity(Player player)
    //{
    //	int Capacity = StorageCapacity;

    //	/*if (MyOptions.Instance?.StomachCapacity != null)
    //          {
    //              Capacity = MyOptions.Instance.StomachCapacity.Value;
    //          }*/
    //	/*if (ModManager.MSC && player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
    //          {
    //              Capacity += 2;
    //          }*/

    //	return Capacity;
    //}

    //// 检查是否有空间
    //public bool HasSpace(Player player)
    //{
    //	return objectsInStomach.Count < GetStomachCapacity(player);
    //}
    #endregion
    #region DumpIL
    /*private static void DumpIL(ILContext il, string title = "IL Dump")
	{
		try
		{
			Logs.Write($"========== {title} ==========", false);

			if (il.Body == null)
			{
				Logs.Write("警告: ILContext.Body 为 null", false);
				return;
			}

			int index = 0;
			foreach (var instr in il.Instrs)
			{
				try
				{
					string formatted = FormatInstruction(instr);
					Logs.Write(formatted, false);
					index++;
				}
				catch (Exception ex)
				{
					Logs.Write($"IL_{instr.Offset:X4}: [无法读取] - {ex.Message}", false);
					index++;
				}
			}
			Logs.Write($"=================================", false);
		}
		catch (Exception ex)
		{
			Logs.Write($"DumpIL 失败: {ex.Message}");
		}
	}

	private static string FormatInstruction(Instruction instr)
	{
		if (instr == null) return "null";

		// 基本格式：偏移量 + 操作码
		string result = $"IL_{instr.Offset:X4}: {instr.OpCode}";

		// 处理操作数
		if (instr.Operand != null)
		{
			try
			{
				// 根据操作数类型格式化
				if (instr.Operand is Instruction target)
				{
					// 跳转指令的目标
					result += $" IL_{target.Offset:X4}";
				}
				else if (instr.Operand is MethodReference method)
				{
					// 方法调用
					string declaringType = method.DeclaringType?.Name ?? "Unknown";
					result += $" {declaringType}::{method.Name}";

					// 如果有泛型参数
					if (method.HasGenericParameters)
					{
						result += $"<{string.Join(",", method.GenericParameters.Select(p => p.Name))}>";
					}
				}
				else if (instr.Operand is MethodDefinition methodDef)
				{
					result += $" {methodDef.DeclaringType?.Name}::{methodDef.Name}";
				}
				else if (instr.Operand is FieldReference field)
				{
					// 字段引用
					string declaringType = field.DeclaringType?.Name ?? "Unknown";
					result += $" {declaringType}::{field.Name}";
				}
				else if (instr.Operand is FieldDefinition fieldDef)
				{
					result += $" {fieldDef.DeclaringType?.Name}::{fieldDef.Name}";
				}
				else if (instr.Operand is ParameterDefinition param)
				{
					// 参数
					result += $" {param.Name ?? $"param_{param.Index}"}";
				}
				else if (instr.Operand is VariableDefinition var)
				{
					// 局部变量
					result += $" {$"V_{var.Index}"}";//var.Name ?? 
				}
				else if (instr.Operand is string str)
				{
					// 字符串常量
					result += $" \"{str}\"";
				}
				else if (instr.Operand is int intVal)
				{
					// 整数常量
					result += $" {intVal}";
				}
				else if (instr.Operand is long longVal)
				{
					result += $" {longVal}";
				}
				else if (instr.Operand is float floatVal)
				{
					result += $" {floatVal}";
				}
				else if (instr.Operand is double doubleVal)
				{
					result += $" {doubleVal}";
				}
				else if (instr.Operand is IMetadataTokenProvider tokenProvider)
				{
					// 通用处理
					result += $" {instr.Operand}";
				}
				else
				{
					// 未知类型，显示类型名
					result += $" [{instr.Operand.GetType().Name}]";
				}
			}
			catch (Exception ex)
			{
				result += $" [操作数错误: {ex.Message}]";
			}
		}

		return result;
	}*/
    #endregion

}