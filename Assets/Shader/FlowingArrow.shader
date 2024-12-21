Shader "Custom/FlowingArrow"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}   // 箭头的主纹理
        _Color("Color", Color) = (1,1,1,1)         // 线的基础颜色
        _FlowSpeed("Flow Speed", Range(-5, 5)) = 1 // 纹理滚动速度（正负可控制方向）
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 属性
            sampler2D _MainTex;       // 主纹理
            float4 _MainTex_ST;       // UV变换
            float4 _Color;            // 基础颜色
            float _FlowSpeed;         // 流动速度

            // 顶点输入
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // 顶点输出
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // 顶点着色器
            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 将世界坐标转换为屏幕坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);     // 计算UV
                return o;
            }

            // 片段着色器
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 让纹理沿着V方向滚动
                uv.x += _Time.y * _FlowSpeed;

                // 采样纹理颜色
                fixed4 col = tex2D(_MainTex, uv);

                // 返回颜色（带基础颜色）
                return col * _Color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
