// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Texture(MaskGray) DephTest"
{
	Properties
	{
		 _CutOff("CutOff",Range(0,1)) = 0
		_MainTex("Base (RGB) Gray, Alpha (A)", 2D) = "white" {}
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite On           //开启深度缓存，以便做控件做基于深度的遮挡剔除
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB        //不输出alpha通道

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;
			float _CutOff;
			struct appdata_t
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				fixed gray : TEXCOORD1;
			};

			float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 col;
				col = tex2D(_MainTex, i.texcoord) * i.color;
				clip(col.a - _CutOff);   //透明度测试
				return col;
			}
			ENDCG
		}
	}
}
