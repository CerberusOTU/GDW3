Shader "Custom/Glow"
{
    Properties 
 {
  _ColorTint("Color Tint", Color) = (1, 1, 1, 1)
  _MainTex("Base (RGB)", 2D) = "white" {}
  _BumpMap("Normal Map", 2D) = "bump" {}
  _Glossiness ("Smoothness", 2D) = "white" {}
  _Metallic ("Metallic", 2D) = "white" {}
  _Occlusion("Occlusion", 2D) = "white" {}
  _Parallax ("Height Map", 2D) = "white" {}
  _Frequency( "Glow Frequency", Float ) = 1.0
  _RimColor("Rim Color", Color) = (1, 1, 1, 1)
  _RimPower("Rim Power", Range(1.0, 6.0)) = 3.0

 }
    SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        struct Input {

   float4 color : Color;
   float2 uv_MainTex;
   float2 uv_BumpMap;
   float2 uv_MetalTex;
   float2 uv_OcclusionTex;
   float2 uv_ParallaxTex;
   float2 uv_SmoothTex;
   float3 viewDir;

  };

  float4 _ColorTint;
  sampler2D _MainTex;
  sampler2D _BumpMap;
  sampler2D _Metallic;
  sampler2D _Smoothness;
  sampler2D _Occlusion;
  sampler2D _Parallax;
  float4 _RimColor;
  float _RimPower;
  half	 _Frequency;
  half	_MinPulseVal;

    void vert(inout appdata_full v,  out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float4 heightMap = tex2Dlod(_Parallax, float4(v.texcoord.xy,0,0));
            //fixed4 heightMap = _HeightMap;
            v.vertex.z += heightMap.b;
        }

  void surf (Input IN, inout SurfaceOutputStandard o) 
  {
    IN.color = _ColorTint;
    half posSin = 0.5 * sin( _Frequency * _Time.y ) + 0.5;
	half pulseMultiplier = posSin * ( 1 - _MinPulseVal ) + _MinPulseVal;
    o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * IN.color;
    o.Normal = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));
    o.Metallic = tex2D (_Metallic, IN.uv_MetalTex).rgb * IN.color;
	o.Smoothness = tex2D (_Smoothness, IN.uv_SmoothTex).rgb * IN.color;
    o.Occlusion = tex2D (_Occlusion, IN.uv_OcclusionTex).rgb * IN.color;
   half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
   o.Emission = _RimColor.rgb * pow(rim, _RimPower) * pulseMultiplier;
  }
        ENDCG
    }
    FallBack "Diffuse"
}
