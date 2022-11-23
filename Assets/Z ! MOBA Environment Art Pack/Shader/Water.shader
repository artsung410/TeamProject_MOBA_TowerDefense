Shader "ZMOBA/Water" {
    Properties {
        _WaterColor ("Water Color", Color) = (0,0.503546,1,1)
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,0.5019608)
        _Fresnelexponent ("Fresnel exponent", Float ) = 4
        _Transparency ("Transparency", Range(0, 1)) = 0.75
        _Glossiness ("Glossiness", Range(0, 1)) = 1
        _SurfaceHighlight ("Surface Highlight", Range(0, 1)) = 0.05
        _Surfacehightlightsize ("Surface hightlight size", Range(0, 1)) = 0
        _SurfaceHightlighttiling ("Surface Hightlight tiling", Float ) = 0.25
        _Depth ("Depth", Range(0, 30)) = 30
        _Depthdarkness ("Depth darkness", Range(0, 1)) = 0.5
        _RimSize ("Rim Size", Range(0, 4)) = 2
        _Rimfalloff ("Rim falloff", Range(0, 5)) = 0.25
        [MaterialToggle] _Worldspacetiling ("Worldspace tiling", Float ) = 0
        _Tiling ("Tiling", Range(0.1, 1)) = 0.9
        _Rimtiling ("Rim tiling", Float ) = 2
        _RefractionAmount ("Refraction Amount", Range(0, 0.2)) = 0.1
        _Wavesspeed ("Waves speed", Range(0, 10)) = 0.75
        _Wavesstrength ("Waves strength", Range(0, 1)) = 0.66
        [NoScaleOffset][Normal]_Normals ("Normals", 2D) = "bump" {}
        [NoScaleOffset]_Shadermap ("Shadermap", 2D) = "black" {}
        _Reflection ("Reflection", Cube) = "_Skybox" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform fixed _RimSize;
            uniform fixed4 _WaterColor;
            uniform fixed4 _RimColor;
            uniform sampler2D _Shadermap;
            uniform fixed _Tiling;
            uniform float _RefractionAmount;
            uniform float _Transparency;
            uniform sampler2D _Normals;
            uniform fixed _Wavesspeed;
            uniform float _Glossiness;
            uniform float _Wavesstrength;
            uniform fixed _Depth;
            uniform samplerCUBE _Reflection;
            uniform fixed _Depthdarkness;
            uniform fixed _Rimtiling;
            uniform fixed _Worldspacetiling;
            uniform fixed _Rimfalloff;
            uniform float _SurfaceHighlight;
            uniform float _Surfacehightlightsize;
            uniform float _SurfaceHightlighttiling;
            uniform float _Fresnelexponent;
            uniform float4 _FresnelColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_8305 = _Time + _TimeEditor;
                float WaveSpeed = (node_8305.g*(_Wavesspeed*0.1));
                fixed mWaveSpeed = WaveSpeed;
                fixed2 Tiling = (lerp( ((-20.0)*o.uv0), mul(unity_ObjectToWorld, v.vertex).rgb.rb, _Worldspacetiling )*(1.0 - _Tiling));
                fixed2 mTiling = Tiling;
                fixed2 WavePanningV = (mTiling+mWaveSpeed*float2(0,1));
                fixed3 node_4911 = UnpackNormal(tex2Dlod(_Normals,float4(WavePanningV,0.0,0)));
                v.vertex.xyz += (v.normal*node_4911.r*_Wavesstrength);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_8305 = _Time + _TimeEditor;
                float WaveSpeed = (node_8305.g*(_Wavesspeed*0.1));
                fixed mWaveSpeed = WaveSpeed;
                fixed2 Tiling = (lerp( ((-20.0)*i.uv0), i.posWorld.rgb.rb, _Worldspacetiling )*(1.0 - _Tiling));
                fixed2 mTiling = Tiling;
                fixed2 WavePanningV = (mTiling+mWaveSpeed*float2(0,1));
                fixed3 node_4911 = UnpackNormal(tex2D(_Normals,WavePanningV));
                fixed2 WavePanningU = (mTiling+mWaveSpeed*float2(0.9,0));
                fixed3 node_49111 = UnpackNormal(tex2D(_Normals,WavePanningU));
                float3 node_3950_nrm_base = node_4911.rgb + float3(0,0,1);
                float3 node_3950_nrm_detail = node_49111.rgb * float3(-1,-1,1);
                float3 node_3950_nrm_combined = node_3950_nrm_base*dot(node_3950_nrm_base, node_3950_nrm_detail)/node_3950_nrm_base.z - node_3950_nrm_detail;
                float3 node_3950 = node_3950_nrm_combined;
                float3 Normals = node_3950;
                float3 normalLocal = Normals;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 Refraction = (float2(node_4911.r,node_49111.g)*_RefractionAmount);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + Refraction;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float Roughness = (saturate((1.0-(1.0-node_4911.r)*(1.0-node_49111.g)))*_Glossiness);
                float gloss = 1.0 - Roughness; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float node_8807 = 0.0;
                float3 specularColor = float3(node_8807,node_8807,node_8807);
                float specularMonochrome;
                float depth = saturate((sceneZ-partZ)/_Depth);
                float RimAllphaMultiply = ((1.0 - pow(saturate((sceneZ-partZ)/_RimSize),_Rimfalloff))*_RimColor.a);
                fixed node_7911 = WaveSpeed;
                fixed2 rimTiling = (Tiling*_Rimtiling);
                fixed2 rimPanningU = (rimTiling+node_7911*float2(1,0));
                float4 rimTexR = tex2D(_Shadermap,rimPanningU);
                fixed2 rimPanningV = (rimTiling+node_7911*float2(0,1));
                float4 rimTexB = tex2D(_Shadermap,rimPanningV);
                float AddRimTextureToMask = (RimAllphaMultiply+(RimAllphaMultiply*(1.0 - (rimTexR.b*rimTexB.b))*_RimColor.a));
                float node_4005 = 1.0;
                float2 HighlightPanningV = (WavePanningV*_SurfaceHightlighttiling);
                float4 node_5469 = tex2D(_Shadermap,HighlightPanningV);
                float2 HightlightPanningU = (WavePanningU*_SurfaceHightlighttiling);
                float4 node_8808 = tex2D(_Shadermap,HightlightPanningU);
                float ClampHighlight = saturate((step(_Surfacehightlightsize,(node_5469.r-node_8808.r))*_SurfaceHighlight));
                float3 diffuseColor = lerp(lerp(_FresnelColor.rgb,lerp(lerp(_WaterColor.rgb,(_WaterColor.rgb*(1.0 - _Depthdarkness)),depth),_RimColor.rgb,saturate(AddRimTextureToMask)),(1.0 - (pow((1.0-max(0,dot(i.normalDir, viewDirection))),_Fresnelexponent)*_FresnelColor.a))),float3(node_4005,node_4005,node_4005),ClampHighlight); // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz)*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (0 + (texCUBE(_Reflection,viewReflectDirection).rgb*_Glossiness));
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,(saturate(( lerp(_Transparency,1.0,AddRimTextureToMask) > 0.5 ? (1.0-(1.0-2.0*(lerp(_Transparency,1.0,AddRimTextureToMask)-0.5))*(1.0-depth)) : (2.0*lerp(_Transparency,1.0,AddRimTextureToMask)*depth) ))+ClampHighlight)),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform fixed _RimSize;
            uniform fixed4 _WaterColor;
            uniform fixed4 _RimColor;
            uniform sampler2D _Shadermap;
            uniform fixed _Tiling;
            uniform float _RefractionAmount;
            uniform float _Transparency;
            uniform sampler2D _Normals;
            uniform fixed _Wavesspeed;
            uniform float _Glossiness;
            uniform float _Wavesstrength;
            uniform fixed _Depth;
            uniform fixed _Depthdarkness;
            uniform fixed _Rimtiling;
            uniform fixed _Worldspacetiling;
            uniform fixed _Rimfalloff;
            uniform float _SurfaceHighlight;
            uniform float _Surfacehightlightsize;
            uniform float _SurfaceHightlighttiling;
            uniform float _Fresnelexponent;
            uniform float4 _FresnelColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_8305 = _Time + _TimeEditor;
                float WaveSpeed = (node_8305.g*(_Wavesspeed*0.1));
                fixed mWaveSpeed = WaveSpeed;
                fixed2 Tiling = (lerp( ((-20.0)*o.uv0), mul(unity_ObjectToWorld, v.vertex).rgb.rb, _Worldspacetiling )*(1.0 - _Tiling));
                fixed2 mTiling = Tiling;
                fixed2 WavePanningV = (mTiling+mWaveSpeed*float2(0,1));
                fixed3 node_4911 = UnpackNormal(tex2Dlod(_Normals,float4(WavePanningV,0.0,0)));
                v.vertex.xyz += (v.normal*node_4911.r*_Wavesstrength);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_8305 = _Time + _TimeEditor;
                float WaveSpeed = (node_8305.g*(_Wavesspeed*0.1));
                fixed mWaveSpeed = WaveSpeed;
                fixed2 Tiling = (lerp( ((-20.0)*i.uv0), i.posWorld.rgb.rb, _Worldspacetiling )*(1.0 - _Tiling));
                fixed2 mTiling = Tiling;
                fixed2 WavePanningV = (mTiling+mWaveSpeed*float2(0,1));
                fixed3 node_4911 = UnpackNormal(tex2D(_Normals,WavePanningV));
                fixed2 WavePanningU = (mTiling+mWaveSpeed*float2(0.9,0));
                fixed3 node_49111 = UnpackNormal(tex2D(_Normals,WavePanningU));
                float3 node_3950_nrm_base = node_4911.rgb + float3(0,0,1);
                float3 node_3950_nrm_detail = node_49111.rgb * float3(-1,-1,1);
                float3 node_3950_nrm_combined = node_3950_nrm_base*dot(node_3950_nrm_base, node_3950_nrm_detail)/node_3950_nrm_base.z - node_3950_nrm_detail;
                float3 node_3950 = node_3950_nrm_combined;
                float3 Normals = node_3950;
                float3 normalLocal = Normals;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 Refraction = (float2(node_4911.r,node_49111.g)*_RefractionAmount);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + Refraction;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float Roughness = (saturate((1.0-(1.0-node_4911.r)*(1.0-node_49111.g)))*_Glossiness);
                float gloss = 1.0 - Roughness; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float node_8807 = 0.0;
                float3 specularColor = float3(node_8807,node_8807,node_8807);
                float specularMonochrome;
                float depth = saturate((sceneZ-partZ)/_Depth);
                float RimAllphaMultiply = ((1.0 - pow(saturate((sceneZ-partZ)/_RimSize),_Rimfalloff))*_RimColor.a);
                fixed node_7911 = WaveSpeed;
                fixed2 rimTiling = (Tiling*_Rimtiling);
                fixed2 rimPanningU = (rimTiling+node_7911*float2(1,0));
                float4 rimTexR = tex2D(_Shadermap,rimPanningU);
                fixed2 rimPanningV = (rimTiling+node_7911*float2(0,1));
                float4 rimTexB = tex2D(_Shadermap,rimPanningV);
                float AddRimTextureToMask = (RimAllphaMultiply+(RimAllphaMultiply*(1.0 - (rimTexR.b*rimTexB.b))*_RimColor.a));
                float node_4005 = 1.0;
                float2 HighlightPanningV = (WavePanningV*_SurfaceHightlighttiling);
                float4 node_5469 = tex2D(_Shadermap,HighlightPanningV);
                float2 HightlightPanningU = (WavePanningU*_SurfaceHightlighttiling);
                float4 node_8808 = tex2D(_Shadermap,HightlightPanningU);
                float ClampHighlight = saturate((step(_Surfacehightlightsize,(node_5469.r-node_8808.r))*_SurfaceHighlight));
                float3 diffuseColor = lerp(lerp(_FresnelColor.rgb,lerp(lerp(_WaterColor.rgb,(_WaterColor.rgb*(1.0 - _Depthdarkness)),depth),_RimColor.rgb,saturate(AddRimTextureToMask)),(1.0 - (pow((1.0-max(0,dot(i.normalDir, viewDirection))),_Fresnelexponent)*_FresnelColor.a))),float3(node_4005,node_4005,node_4005),ClampHighlight); // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * (saturate(( lerp(_Transparency,1.0,AddRimTextureToMask) > 0.5 ? (1.0-(1.0-2.0*(lerp(_Transparency,1.0,AddRimTextureToMask)-0.5))*(1.0-depth)) : (2.0*lerp(_Transparency,1.0,AddRimTextureToMask)*depth) ))+ClampHighlight),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform fixed _Tiling;
            uniform sampler2D _Normals;
            uniform fixed _Wavesspeed;
            uniform float _Wavesstrength;
            uniform fixed _Worldspacetiling;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_8305 = _Time + _TimeEditor;
                float WaveSpeed = (node_8305.g*(_Wavesspeed*0.1));
                fixed mWaveSpeed = WaveSpeed;
                fixed2 Tiling = (lerp( ((-20.0)*o.uv0), mul(unity_ObjectToWorld, v.vertex).rgb.rb, _Worldspacetiling )*(1.0 - _Tiling));
                fixed2 mTiling = Tiling;
                fixed2 WavePanningV = (mTiling+mWaveSpeed*float2(0,1));
                fixed3 node_4911 = UnpackNormal(tex2Dlod(_Normals,float4(WavePanningV,0.0,0)));
                v.vertex.xyz += (v.normal*node_4911.r*_Wavesstrength);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
