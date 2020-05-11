Shader "Custom/Toon"
{
    Properties
    {
		[HDR]
		_AmbientColor ("AmbientColor", Color) = (0.4, 0.4, 0.4, 1)
		[HDR]
		_SpecularColor ("SpecularColor", Color) = (0.9, 0.9, 0.9, 1)
		_Glossiness ("Glossiness", Float) = 32
		[HDR]
		_OutlineColor("OutlineColor", Color) = (1, 1, 1, 1)
		_OutlineThickness("Outline Thickness", Range(0,1)) = 0.716 
		_OutlineThreshold("Outline Threshold", Range(0, 1)) = 0.1
		_Color ("Color", Color) = (0.5, 0.65, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
		[Normal]_BumpMap ("Normal Map", 2D) = "bump" {}
		_NormalIntensity ("Normal Intensity", Range(0,1)) = 0.5
    }
    SubShader
    {
		Tags
		{
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				half3 tspace0 : TEXCOORD1;
				half3 tspace1 : TEXCOORD2;
				half3 tspace2 : TEXCOORD3;
                float4 pos : SV_POSITION;
				float3 viewDir : TEXCOORD4;
				float3 normalDir : TEXCOORD5;
				SHADOW_COORDS(6)
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;

            v2f vert (appdata_tan v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
				o.normalDir = v.normal;
				half3 wNormal = UnityObjectToWorldNormal(v.normal);
				half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
				half tanSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 wBitan = cross(wNormal, wTangent) * tanSign;
				o.tspace0 = half3(wTangent.x, wBitan.x, wNormal.x);
				o.tspace1 = half3(wTangent.y, wBitan.y, wNormal.y);
				o.tspace2 = half3(wTangent.z, wBitan.z, wNormal.z);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				TRANSFER_SHADOW(o)
                return o;
            }

			float4 _AmbientColor;
			float4 _SpecularColor;
			float _Glossiness;
			float4 _OutlineColor;
			float _OutlineThickness;
			float _OutlineThreshold;
			float4 _Color;
			float _NormalIntensity;

            float4 frag (v2f i) : SV_Target
            {
				half2 uv_normal = TRANSFORM_TEX(i.uv, _BumpMap);
				half3 tnormal = UnpackNormal(tex2D(_BumpMap, uv_normal));

				half3 worldNormal;
				worldNormal.x = dot(i.tspace0, tnormal);
				worldNormal.y = dot(i.tspace1, tnormal);
				worldNormal.z = dot(i.tspace2, tnormal);

				float3 normal = normalize(worldNormal);

				normal *= _NormalIntensity;

				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float3 viewDir = normalize(i.viewDir);
				float3 hV = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, hV);

				float shadow = SHADOW_ATTENUATION(i);

				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
				float specIntensity = pow(NdotH * lightIntensity, pow(_Glossiness, 2));
				float specSmooth = smoothstep(0.000, 0.01, specIntensity);
				float4 spec = specSmooth * _SpecularColor;

				float4 rim = 1 - dot(viewDir, normal);
				float4 outlineIntensity = rim * pow(NdotL,_OutlineThreshold);
				 outlineIntensity = smoothstep(_OutlineThickness-0.01, _OutlineThickness+0.01, outlineIntensity);
				float4 outline = outlineIntensity * _OutlineColor;

				float4 light = lightIntensity * _LightColor0;

                fixed4 sample = tex2D(_MainTex, i.uv);
				fixed4 c;
				c.rgb = sample.rgb;
				c.a = sample.a;

                return _Color * c * (light + _AmbientColor + spec + outline);
            }
            ENDCG
        }
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
