Shader "Custom/WorldAlign"
{
    Properties{
    _Color("Main Color", Color) = (1,1,1,1)
    _MainTex("Base (RGB)", 2D) = "white" {}
    _WallTex("Base (RGB)", 2D) = "white" {}
    _Scale("Texture Scale", Float) = 1.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _WallTex;
        fixed4 _Color;
        float _Scale;

        struct Input
        {
            float3 worldNormal;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            float3 uvs = IN.worldPos.xyz * _Scale;
            float3 blending = saturate(abs(IN.worldNormal.xyz) - 0.2); // Change the 0.2 value to adjust blending
            blending = pow(blending, 2.0); // Change the 2.0 value to adjust blending
            blending /= dot(blending, float3(1.0, 1.0, 1.0));
            float4 c = blending.x * tex2D(_WallTex, uvs.yz);
            c = blending.y * tex2D(_MainTex, uvs.xz) + c; // Single MAD
            c = blending.z * tex2D(_WallTex, uvs.xy) + c; // Single MAD

            o.Albedo = c.rgb * _Color;
        }
        ENDCG
    }

        Fallback "VertexLit"
}
