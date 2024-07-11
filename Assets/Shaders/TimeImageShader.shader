Shader "Custom/TimeImageShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Distorsion ("Distorsion (RGB)", 2D) = "white" { }
        _Intensity ("Intensity", Range(0.01,10)) = 7.0
        _Velocity("Velocity", Range(0.01, 100)) = 1.0
    }
    SubShader
    {
    Pass
        {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma vertex vsMain
        #pragma fragment psMain
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _Distorsion;
        float _Intensity;
        float _Velocity;

        struct VsOut
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos: TEXCOORD1;
            };

        VsOut vsMain(appdata_base v)
        {
            VsOut o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            return o;
        }

        float4 psMain(VsOut i) : COLOR
        {
            float4 distorsion = tex2D(_Distorsion, i.uv);
            float rateX = (distorsion.r)/_Intensity - (0.5/_Intensity);
            float rateY = (distorsion.g)/_Intensity - (0.5/_Intensity);
            float4 baseColor = tex2D(_MainTex, i.uv + float2(sin((_Time.z + abs(rateX)*100)*_Velocity/5)*rateX, cos((_Time.z+ abs(rateY)*100)*_Velocity/5)*rateY));
            return baseColor;
        }
        
        ENDCG
    }
    }
}
