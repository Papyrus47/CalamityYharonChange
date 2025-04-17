using CalamityYharonChange;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;

namespace CalamityYharonChange
{
    public static class Easing
    {
        public static float Linear(float pos, params float[] points)
        {
            pos = MathHelper.Clamp(pos, 0, 1);

            float pos1 = pos * (points.Length - 1);
            int pointID = (int)pos1;

            return MathHelper.Lerp(points[pointID], points[pointID + 1], pos1 - pointID);
        }
        public static float Cubic(float pos, params float[] points)
        {
            pos = MathHelper.Clamp(pos, 0, 1);
            if (pos == 1)
            {
                return points[^1];
            }
            float pos1 = pos * (points.Length - 1);
            int pointID = (int)pos1;

            return MathHelper.Lerp(points[pointID], points[pointID + 1], MathF.Pow(pos1 - pointID, 0.33f));
        }
    }
    public static class Helper
    {
        //[DllImport("User32.dll")]
        //public static extern bool SetCursorPos(int x, int y);

        public static Vector2 ScreenCenter => Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2;
        //finalColor=sColor+desColor
        public static BlendState NewAdditive => NewBlendState("BlendState.Additive", Blend.SourceAlpha, Blend.SourceAlpha, Blend.One, Blend.One);
        //finalColor=
        public static BlendState NewAlphaBlend => NewBlendState("BlendState.AlphaBlend", Blend.One, Blend.One, Blend.InverseSourceAlpha, Blend.InverseSourceAlpha);
        public static BlendState NewNonPremultiplied => NewBlendState("BlendState.NonPremultiplied", Blend.SourceAlpha, Blend.SourceAlpha, Blend.InverseSourceAlpha, Blend.InverseSourceAlpha);
        public static BlendState NewOpaque => NewBlendState("BlendState.Opaque", Blend.One, Blend.One, Blend.Zero, Blend.Zero);

        public static void DrawOnCenter(Texture2D tex, Vector2 pos, Color color, float rotation, float scale)
        {
            Main.spriteBatch.Draw(tex, pos, null, color, rotation, tex.Size() / 2, scale, 0, 0);
        }
        public static void DrawOnCenter(string texpath, Vector2 pos, Color color, float rotation, float scale)
        {

            Texture2D tex = ModContent.Request<Texture2D>(texpath).Value;
            Main.spriteBatch.Draw(tex, pos, null, color, rotation, tex.Size() / 2, scale, 0, 0);
        }
        public static void DrawOnCenter(string texpath, Vector2 pos, Color color, float rotation, Vector2 scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>(texpath).Value;
            Main.spriteBatch.Draw(tex, pos, null, color, rotation, tex.Size() / 2, scale, 0, 0);
        }
        public static void DrawOnCenter(Texture2D tex, Vector2 pos, Color color, float rotation, Vector2 scale)
        {
            Main.spriteBatch.Draw(tex, pos, null, color, rotation, tex.Size() / 2, scale, 0, 0);
        }
        public static float SmoothStep(float t1, float t2, float x)
        {
            x = MathHelper.Clamp((x - t1) / (t2 - t1), 0.0f, 1.0f);
            return x * x * (3 - 2 * x);
        }

        public static bool OwnerNPCAvailable(NPC npc, int npctype)
        {
            return npc.active && npc.type == npctype;
        }

        public static void SetWCSMatrix(Effect e)
        {
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
            e.Parameters["uTransform"].SetValue(model * projection);
        }
        public static BlendState NewBlendState(string name, Blend colorSourceBlend, Blend alphaSourceBlend, Blend colorDestBlend, Blend alphaDestBlend)
        {
            BlendState bs = new BlendState();

            bs.ColorSourceBlend = Blend.One;
            bs.ColorDestinationBlend = Blend.Zero;
            bs.ColorBlendFunction = BlendFunction.Add;

            bs.AlphaSourceBlend = Blend.One;
            bs.AlphaDestinationBlend = Blend.Zero;
            bs.AlphaBlendFunction = BlendFunction.Add;

            bs.ColorWriteChannels = ColorWriteChannels.All;
            bs.ColorWriteChannels1 = ColorWriteChannels.All;
            bs.ColorWriteChannels2 = ColorWriteChannels.All;
            bs.ColorWriteChannels3 = ColorWriteChannels.All;
            bs.BlendFactor = Color.White;
            bs.MultiSampleMask = -1;

            bs.Name = name;
            bs.ColorSourceBlend = colorSourceBlend;
            bs.AlphaSourceBlend = alphaSourceBlend;
            bs.ColorDestinationBlend = colorDestBlend;
            bs.AlphaDestinationBlend = alphaDestBlend;

            return bs;
        }
        public static Matrix WCSMatrix()
        {
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
            return model * projection;
        }
        public static void TrackOldValue<T>(T[] array, T curValue)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                array[i] = array[i - 1];
            }
            array[0] = curValue;
        }
        public static void LerpValue(this ref float value, float b, float n)
        {
            value = MathHelper.Lerp(value, b, n);
        }
        public static void LerpValue(this ref Vector2 value, Vector2 b, float n)
        {
            value = Vector2.Lerp(value, b, n);
        }
        public static double StdGaussian()
        {
            var rand = new Random();
            double u = -2 * Math.Log(rand.NextDouble());
            double v = 2 * Math.PI * rand.NextDouble();
            return Math.Sqrt(u) * Math.Cos(v);
        }

        public static double Gaussian(double mu, double sigma)
        {
            return StdGaussian() * sigma + mu;
        }

        public static Vector3 AddXY(this Vector3 vec, Vector2 add)
        {
            return new Vector3(vec.X + add.X, vec.Y + add.Y, vec.Z);
        }

        public static Vector3 ToScreenCoords(this Vector3 vec)
        {
            vec.X -= Main.screenPosition.X;
            vec.Y -= Main.screenPosition.Y;
            return vec;
        }

        public static bool IsInScreen(Vector2 pos, int fluff = 50)
        {
            return pos.X > Main.screenPosition.X - fluff && pos.X < Main.screenPosition.X + Main.screenWidth + fluff && pos.Y > Main.screenPosition.Y - fluff && pos.Y < Main.screenPosition.Y + Main.screenHeight + fluff;
        }

        public static string RandomString(int length)
        {
            string str = "abcdefghmnopqrstFGHMNOPQRSTUVWXYZ~!@#$%^&*()_+{}|[]<>;':,./?";
            string sum = "";
            for (int i = 0; i < length; i++)
            {
                sum += str.Substring(Main.rand.Next(str.Length - 1), 1);
            }
            return sum;
        }

        public static float GetLightIntensity(Vector2 pos)
        {
            Color c = Lighting.GetColor((pos / 16).ToPoint());
            return (c.R + c.G + c.B) / 766f;
        }
        /*
        public static void UseColorlizeShader(float offset = 0)
        {
            MEAC.Colorize.Parameters["uOffset"].SetValue(offset);

            MEAC.Colorize.CurrentTechnique.Passes[0].Apply();
        }*/

        public static void MoveTo(this NPC npc, Vector2 targetPos, float Speed, float n)
        {
            Vector2 targetVec = Normalize2(targetPos - npc.Center) * Speed;
            npc.velocity = (npc.velocity * n + targetVec) / (n + 1);
        }

        public static float GetMeleeSpeed(Player player, float max = 100)
        {
            return Math.Min((player.GetAttackSpeed(DamageClass.Melee) - 1) * 100, max);
        }

        public static Projectile NewProjectileDirect(bool? nul, Vector2 pos, Vector2 vel, int type, int damage, int kno, int owner, float ai0, float ai1)
        {
            return Projectile.NewProjectileDirect(null, pos, vel, type, damage, kno, owner, ai0, ai1);
        }

        public static bool CanBlockSmash(Tile tile)
        {
            return Main.tileSolidTop[tile.TileType] || Main.tileSolid[tile.TileType] && tile.HasTile;
        }
        public static float HeightFromGround(Vector2 position, out Vector2 ground)
        {
            ground = Vector2.Zero;
            Point p = position.ToTileCoordinates();
            int i1 = 0;
            for (; i1 < 100; i1 += 2)
            {
                int x = p.X;
                int y = p.Y + i1;
                if (x > 0 && y > 0 && x < Main.maxTilesX && y < Main.maxTilesY)
                {
                    Tile tile = Main.tile[x, y];
                    if (CanBlockSmash(tile))
                    {
                        ground = new Vector2(x, y) * 16;
                        break;
                    }
                }
            }
            return i1;
        }
        public static float HeightFromGround(Vector2 position)
        {
            Point p = position.ToTileCoordinates();
            int i1 = 0;
            for (; i1 < 200; i1++)
            {
                int x = p.X;
                int y = p.Y + i1;
                if (x > 0 && y > 0 && x < Main.maxTilesX && y < Main.maxTilesY)
                {
                    Tile tile = Main.tile[x, y];
                    if (CanBlockSmash(tile))
                    {
                        break;
                    }
                }
            }
            return i1;
        }
        public static float HeightFromGround(Player player, int width = 32, int height = 48)
        {
            Point p = player.position.ToTileCoordinates();
            int i1 = 0, i2 = 0;
            for (; i1 < 20; i1++)
            {
                int x = p.X;
                int y = p.Y + i1 * (int)player.gravDir;
                if (x > 0 && y > 0 && x < Main.maxTilesX && y < Main.maxTilesY)
                {
                    Tile tile = Main.tile[x, y];
                    if (CanBlockSmash(tile))
                    {
                        break;
                    }
                }
            }
            for (; i2 < 20; i2++)
            {
                int x = p.X + 1;
                int y = p.Y + i2 * (int)player.gravDir;
                if (x > 0 && y > 0 && x < Main.maxTilesX && y < Main.maxTilesY)
                {
                    Tile tile = Main.tile[x, y];
                    if (CanBlockSmash(tile))
                    {
                        break;
                    }
                }
            }
            return Math.Min(i1, i2) * 16f - height;
        }

        public static void SpawnParticleSystem(ParticleOrchestraType type, Vector2 pos, Vector2 vel)
        {
            ParticleOrchestrator.RequestParticleSpawn(true, type, new ParticleOrchestraSettings
            {
                PositionInWorld = pos,
                MovementVector = vel
            });
        }

        public static float LaserLength(Vector2 pos, float dir, float maxlength, int width = 1, float n = 5)
        {
            float w = maxlength;
            Vector2 unit = dir.ToRotationVector2();
            if (Collision.CanHitLine(pos, width, width, pos + unit * w, width, width))
            {
                return w;
            }
            w /= 2;
            float length = w;
            while (length > n)
            {
                length /= 2;
                if (Collision.CanHitLine(pos, width, width, pos + unit * w, width, width))
                {
                    w += length;
                }
                else
                {
                    w -= length;
                }
            }
            return w;
        }

        public static Vector2 Normalize2(Vector2 vector2)
        {
            return vector2 != Vector2.Zero ? Vector2.Normalize(vector2) : new Vector2(0, 0.001f);
        }

        public static Vector2 DirectionTo2(this Entity entity, Vector2 t)
        {
            return t - entity.Center != Vector2.Zero ? Vector2.Normalize(t - entity.Center) : Vector2.Zero;
        }

        public static Vector2 DirectionFrom2(this Entity entity, Vector2 t)
        {
            return t - entity.Center != Vector2.Zero ? -Vector2.Normalize(t - entity.Center) : Vector2.Zero;
        }

        public static Vector2 Vxr(float rot)
        {
            return new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
        }


        public static void SpinAI(Entity entity, Vector2 center, float v, bool changeVelocity = true)//entity旋转的AI
        {
            Vector2 oldPos = entity.Center;
            entity.Center = center + (oldPos - center).RotatedBy(v);
            if (changeVelocity)
            {
                entity.velocity = entity.Center - oldPos;
                entity.position -= entity.velocity;
            }
        }

        public static Color HsvToRgb(float H, float S = 1, float V = 1)//HSV转RGB
        {
            int i;
            float R = 0, G = 0, B = 0, f, a, b, c;
            H /= 60;
            i = (int)H;
            f = H - i;
            a = V * (1 - S);
            b = V * (1 - S * f);
            c = V * (1 - S * (1 - f));
            switch (i)
            {
                case 0:
                    R = V;
                    G = c;
                    B = a;
                    break;

                case 1:
                    R = b;
                    G = V;
                    B = a;
                    break;

                case 2:
                    R = a;
                    G = V;
                    B = c;
                    break;

                case 3:
                    R = a;
                    G = b;
                    B = V;
                    break;

                case 4:
                    R = c;
                    G = a;
                    B = V;
                    break;

                case 5:
                    R = V;
                    G = a;
                    B = b;
                    break;
            }
            return new Color(R, G, B);
        }

        public static Vector3 Vec3RotBy(string str, Vector3 vec, float rot)//三维向量绕某轴旋转
        {
            float cos = (float)Math.Cos(rot);
            float sin = (float)Math.Sin(rot);
            Matrix matrix = str == "X"
                ? new Matrix(1, 0, 0, 0,
                                    0, cos, -sin, 0,
                                     0, sin, cos, 0,
                                    0, 0, 0, 1)
                : str == "Y"
                    ? new Matrix(cos, 0, sin, 0,
                                                             0, 1, 0, 0,
                                                          -sin, 0, cos, 0,
                                                             0, 0, 0, 1)
                    : new Matrix(cos, -sin, 0, 0,
                                                    sin, cos, 0, 0,
                                                     0, 0, 1, 0,
                                                    0, 0, 0, 1);
            return Vector3.Transform(vec, matrix);
        }

        public static Vector2 VerticalVec(Vector2 vec)
        {
            return new Vector2(-vec.Y, vec.X);
        }

        public static float ToRotation2(this Vector2 vec)
        {
            if (vec == Vector2.Zero)
                return 0;
            return vec.ToRotation();
        }

        public static void AdditiveDraw(SpriteBatch spriteBatch, Action action)//Additive混合模式绘制
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            action();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static int NewNPC(bool i, Vector2 Position, int type, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0)
        {
            return NPC.NewNPC(null, (int)Position.X, (int)Position.Y, type, 0, ai0, ai1, ai2, ai3);
        }

        public static void HomingTo(Projectile proj, NPC npc, float vel, float n)//追踪敌人
        {
            proj.velocity = (proj.velocity * n + proj.DirectionTo(npc.Center) * vel) / (n + 1);
        }

        public static float AngleDiffer(float a, float b)
        {
            float norm = (a - b) % MathHelper.TwoPi;
            return Math.Min(MathHelper.TwoPi - norm, norm);
        }

        public static void HomingToClosest(Projectile proj, float vel, float n, float MaxDistance = 2000)//追踪敌人
        {
            NPC target = null;
            float maxDis = MaxDistance;
            foreach (NPC npc in Main.npc)
            {
                float dis = Vector2.Distance(npc.Center, proj.Center);
                if (dis < maxDis && !npc.friendly && npc.active)
                {
                    maxDis = dis;
                    target = npc;
                }
            }
            if (target != null)
            {
                proj.velocity = (proj.velocity * n + proj.DirectionTo(target.Center) * vel) / (n + 1);
            }
        }
        public static bool TargetPlayer(Vector2 center, int range, out Player player)
        {
            int id = Player.FindClosest(center, range, range);
            if (id >= 0 && id < 255)
            {
                player = Main.player[id];
                return true;
            }
            player = new Player();
            return false;
        }
        public static int TargetClosestNPC(Vector2 pos, float maxDis = 2000)
        {
            NPC target = null;

            foreach (NPC npc in Main.npc)
            {
                float dis = Vector2.Distance(npc.Center, pos);
                if (dis < maxDis && !npc.friendly && npc.active)
                {
                    maxDis = dis;
                    target = npc;
                }
            }
            return target != null ? target.whoAmI : -1;
        }

        public static void TargetClosestNPC(Projectile proj, float MaxDistance = 2000)//寻敌
        {
            NPC target = null;
            float maxDis = MaxDistance;
            foreach (NPC npc in Main.npc)
            {
                float dis = Vector2.Distance(npc.Center, proj.Center);
                if (dis < maxDis && !npc.friendly && npc.active)
                {
                    maxDis = dis;
                    target = npc;
                }
            }
            proj.ai[1] = target != null ? target.whoAmI : -1;
        }

        public static Vector2 Vector2Elipse(float x, float y, float rot1, float rot2 = 0)
        {
            Vector2 vec = Vxr(rot1);
            vec.X *= x;
            vec.Y *= y;
            if (rot2 != 0)
            {
                vec = vec.RotatedBy(rot2);
            }
            return vec;
        }

        public static Vector2 Vector2Elipse2(float radius, float rot0, float rot1, float rot2 = 0, float viewZ = 500)
        {
            Vector3 v = Vec3RotBy("Z", Vector3.UnitX, -rot0) * radius;
            v = Vec3RotBy("X", v, rot1);
            if (rot2 != 0)
            {
                v = Vec3RotBy("Z", v, rot2);
            }

            float k = -viewZ / (v.Z - viewZ);
            //Vector2 start = new Vector2(v.X, v.Y);
            //Vector2 result=Vector2.Zero;
            //result.X = v.X - (1 - k) * v.X;
            return k * new Vector2(v.X, v.Y);
        }

        public static Vector2 Projection(this Vector3 vec, Vector2 center, float viewZ = 500)
        {
            float k1 = -viewZ / (vec.Z - viewZ);
            Vector2 v = new Vector2(vec.X, vec.Y);
            return v + (k1 - 1) * (v - center);
        }

        public static Vector2 Projection(this Vector3 vec, Vector2 center, out float k, float viewZ = 500)
        {
            float k1 = -viewZ / (vec.Z - viewZ);
            Vector2 v = new Vector2(vec.X, vec.Y);
            k = k1;
            return v + (k1 - 1) * (v - center);
        }

        public static float[] SetRandomArr(float[] arr)
        {
            float[] c = arr;
            for (int i = 0; i < c.Length; i++)
            {
                int r = Main.rand.Next(c.Length);
                (c[r], c[i]) = (c[i], c[r]);
            }
            return c;
        }

        public static Vector3 RotatedBy(this Vector3 v, Vector3 u, float ang)//v以u为轴旋转
        {
            float cos = (float)Math.Cos(ang);
            return v * cos + Vector3.Dot(v, u) * u * (1 - cos) + Vector3.Cross(u, v) * (float)Math.Sin(ang);
        }
        public static float AngleBetween(Vector2 u, Vector2 v)//求两向量之间的夹角
        {
            if (u == Vector2.Zero || v == Vector2.Zero)
                return 0;
            float d = Vector2.Dot(u, v) / (u.Length() * v.Length());
            if (d >= 1)
                return 0;
            else if (d <= -1)
                return 3.1415f;
            else
            {
                float a = (float)Math.Acos(d);
                return a;
            }
        }
        public static float AngleBetween(Vector3 u, Vector3 v)//求两向量之间的夹角
        {
            float a = (float)Math.Acos(Vector3.Dot(u, v) / (u.Length() * v.Length()));

            return a;
        }

        public static Color GetLight(Vector2 pos, Color orig)
        {
            return Lighting.GetColor((int)(pos.X / 16), (int)(pos.Y / 16), orig);
        }

        /*
        public static void DrawTrail(SpriteBatch spriteBatch,string passName,MEAC.VertexInfo[] vertexInfos,float t,Texture2D tex0,Texture2D tex1,Texture2D tex2=null)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);

            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            MEAC.Trail.Parameters["uTransform"].SetValue(model * projection);
            MEAC.Trail.Parameters["t"].SetValue(t);
            Main.graphics.GraphicsDevice.Textures[0] = tex0;
            Main.graphics.GraphicsDevice.Textures[1] = tex1;
            Main.graphics.GraphicsDevice.Textures[2] = tex2;

            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointClamp;

            MEAC.Trail.CurrentTechnique.Passes[passName].Apply();
            if (vertexInfos.Length >= 3)
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertexInfos, 0, vertexInfos.Length - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }*/

        public static Color[,] GetColors(Texture2D tex)
        {
            Color[] colors = new Color[tex.Width * tex.Height];
            tex.GetData(colors, 0, tex.Width * tex.Height);

            Color[,] colors2 = new Color[tex.Width, tex.Height];
            for (int i = 0; i < colors.Length; i++)
            {
                int a = i % tex.Width;
                int b = (int)Math.Floor((double)i / tex.Width);
                colors2[a, b] = colors[i];
            }
            return colors2;
        }

        /*Texture2D createCircleText(int r,int h)
{
    Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, r, r);
    Color[] colorData = new Color[r * r];

    float diam = r / 2f;
    float diamsq = diam * diam;

    for (int x = 0; x < r; x++)
    {
        for (int y = 0; y < r; y++)
        {
            int index = x * r + y;
            Vector2 pos = new Vector2(x - diam, y - diam);
            if (pos.LengthSquared() <= diamsq&& pos.LengthSquared()>diamsq-h)
            {
                colorData[index] = Color.White;
            }
            else
            {
                colorData[index] = Color.Transparent;
            }
        }
    }
    texture.SetData(colorData);
    return texture;
}*/
    }
}