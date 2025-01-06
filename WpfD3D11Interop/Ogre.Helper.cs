using org.ogre;

namespace OgreEngine
{
    public partial class Ogre : Microsoft.Wpf.Interop.DirectX.D3D11Image
    {
        public enum Engine
        {
            DX9,
            DX11
        }

        class SGResolver : MaterialManager.MaterialManager_Listener
        {
            private ShaderGenerator shadergen;

            public SGResolver(ShaderGenerator shadergen)
            {
                this.shadergen = shadergen;
            }

            public override Technique handleSchemeNotFound(ushort schemeIndex, string schemeName, Material material, ushort lodIndex, Renderable rend)
            {
                if (schemeName != ShaderGenerator.DEFAULT_SCHEME_NAME)
                {
                    return null;
                }

                bool success = shadergen.createShaderBasedTechnique(material, MaterialManager.DEFAULT_SCHEME_NAME, schemeName);

                if (!success)
                {
                    return null;
                }

                shadergen.validateMaterial(schemeName, material.getName(), material.getGroup());

                return material.getTechnique(1);
            }
        }
    }
}
