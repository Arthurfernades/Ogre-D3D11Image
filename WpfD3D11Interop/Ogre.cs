using org.ogre;
using System;
using System.Windows;
using System.Windows.Interop;

namespace OgreEngine
{
    public partial class Ogre : Microsoft.Wpf.Interop.DirectX.D3D11Image
    {        
        public Engine DX;

        private Root root;

        private RenderWindow renderWindow;

        private RenderSystem renderSystem;

        private SceneManager scnMgr;

        private ShaderGenerator shadergen;

        public void Initialize()
        {
            root = new Root("plugins.cfg", "ogre.cfg", "");

            #region Log

            Log log = LogManager.getSingleton().createLog("OgreLog.log", true, true, false);

            #endregion

            #region Puglins / Resources config


            ConfigFile configFile = new ConfigFile();

            configFile.loadDirect("resources.cfg");

            ResourceGroupManager resourceGroupManager = ResourceGroupManager.getSingleton();

            foreach (var section in configFile.getSettingsBySection())
            {
                if (section.Key != null && section.Key != "")
                {
                    string sectionName = section.Key;

                    var settings = configFile.getMultiSetting("Zip", sectionName);

                    foreach (var setting in settings)
                    {
                        resourceGroupManager.addResourceLocation(setting, "Zip", sectionName);
                    }

                    settings = configFile.getMultiSetting("FileSystem", sectionName);

                    foreach (var setting in settings)
                    {
                        resourceGroupManager.addResourceLocation(setting, "FileSystem", sectionName);
                    }
                }
            }

            #endregion

            #region RenderSystem

            bool foundit = false;
            string renderName = "";
            if (DX == Engine.DX9)
            {
                renderName = "Direct3D9 Rendering Subsystem";
            }
            else if (DX == Engine.DX11)
            {
                renderName = "Direct3D11 Rendering Subsystem";
            }

            foreach (RenderSystem rs in root.getAvailableRenderers())
            {
                if (rs.getName() == renderName)
                {
                    root.setRenderSystem(rs);

                    renderSystem = rs;

                    foreach (var c in rs.getConfigOptions())
                    {
                        foreach (var p in c.Value.possibleValues)
                        {
                            LogManager.getSingleton().logMessage(p);
                        }
                    }

                    foundit = true;
                    break;
                }
            }

            if (!foundit)
            {
                throw new Exception("Failed to find a compatible render system.");
            }

            renderSystem.setConfigOption("Full Screen", "No");
            renderSystem.setConfigOption("Video Mode", "1920 x 1080 @ 32-bit colour");
            renderSystem.setConfigOption("Allow NVPerfHUD", "No");
            renderSystem.setConfigOption("FSAA", "0");

            if (DX == Engine.DX9)
            {
                renderSystem.setConfigOption("Floating-point mode", "Consistent");
                renderSystem.setConfigOption("Resource Creation Policy", "Create on active device");
            }
            renderSystem.setConfigOption("VSync", "No");

            #endregion

            root.initialise(false);

            #region Render Window

            IntPtr hWnd = getWindowHandle();

            NameValueMap miscParams = new NameValueMap
            {
                ["FSAA"] = "1",
                ["Full Screen"] = "No",
                ["VSync"] = "No",
                ["sRGB Gamma Conversion"] = "No",
                ["externalWindowHandle"] = hWnd.ToString()
            };

            renderWindow = root.createRenderWindow(
                "Window Forms Ogre", //Render Targer name
                0, // Width
                0, // Height
                false, // Windowed mode
                miscParams
            );

            renderWindow.setAutoUpdated(false);

            #endregion

            resourceGroupManager.initialiseAllResourceGroups();

            MaterialManager.getSingleton().setDefaultTextureFiltering(TextureFilterOptions.TFO_ANISOTROPIC);
            MaterialManager.getSingleton().setDefaultAnisotropy(16);

            #region Shader Generator (DX11)

            if (DX == Engine.DX11)
            {
                ShaderGenerator.initialize();
                shadergen = ShaderGenerator.getSingleton();

                SGResolver sgres = new SGResolver(shadergen);
                MaterialManager.getSingleton().addListener(sgres);


                RenderState renderState = shadergen.getRenderState("ShaderGeneratorDefaultScheme");
                renderState.addTemplateSubRenderState(shadergen.createSubRenderState("SGX_PerPixelLighting"));

                scnMgr = root.createSceneManager();
                shadergen.addSceneManager(scnMgr);

            }
            else
            {
                scnMgr = root.createSceneManager();
            }

            #endregion

        }

        public IntPtr getWindowHandle()
        {
            IntPtr hWnd = IntPtr.Zero;

            foreach (PresentationSource source in PresentationSource.CurrentSources)
            {
                var hwndSource = (source as HwndSource);
                if (hwndSource != null)
                {
                    hWnd = hwndSource.Handle;
                    break;
                }
            }

            return hWnd;
        }
    }
}