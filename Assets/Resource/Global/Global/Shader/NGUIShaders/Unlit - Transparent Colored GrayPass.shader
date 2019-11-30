Shader "Unlit/Transparent Colored GrapyPass"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
	}

		
		SubShader
		{
			Tags
			{
				"Queue" = "Transparent+300"  //修改渲染队列，保证优先绘制
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}

			//黑白颜色Pass
			Pass
			{
				ZWrite On            //开启深度写入
				ZTest Greater		 //修改深度测试条件						
				Offset -1, -1        //深度偏移(保证黑白色在最上边)
				Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag			
				#include "UnityCG.cginc"

				sampler2D _MainTex;

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
				};

				v2f o;

				v2f vert(appdata_t v)
				{
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = v.texcoord;
					return o;
				}

				fixed4 frag(v2f IN) : COLOR
				{
					fixed4 MainTexcol = tex2D(_MainTex, IN.texcoord);
					fixed4 col = dot(MainTexcol.rgb, fixed3(.222, .707, .071));       //绘制黑白色图片
					col.a = MainTexcol.a;
					return col;
				}
			ENDCG
			}


			//正常颜色Pass
			Pass
			{
				ZWrite On
				Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag			
				#include "UnityCG.cginc"

				sampler2D _MainTex;

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				v2f o;

				v2f vert(appdata_t v)
				{
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = v.texcoord;
					o.color = fixed4(v.color.rgb, v.color.a);
					return o;
				}

				fixed4 frag(v2f IN) : COLOR
				{
					return tex2D(_MainTex, IN.texcoord);
				}
			ENDCG
			}


		}
}
