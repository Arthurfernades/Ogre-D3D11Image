using org.ogre;

namespace OgreEngine
{
    public partial class Ogre : Microsoft.Wpf.Interop.DirectX.D3D11Image
    {
        private Camera cam;
        public void CreateScene(int i)
        {
            switch (i)
            {
                case 1:
                    SinabdScene();
                    break;
                case 2:
                    OgreHeadScene();
                    break;
            }
        }

        private void SinabdScene()
        {
            #region Ambient Light

            scnMgr.setAmbientLight(new ColourValue(0.2f, 0.2f, 0.2f));

            #endregion

            #region Camera

            cam = scnMgr.createCamera("myCam");
            cam.setAutoAspectRatio(true);
            cam.setNearClipDistance(5);
            SceneNode camnode = scnMgr.getRootSceneNode().createChildSceneNode();
            camnode.attachObject(cam);

            #endregion

            #region Camera Man

            CameraMan camMan = new CameraMan(camnode);
            camMan.setStyle(CameraStyle.CS_ORBIT);
            camMan.setYawPitchDist(new Radian(0), new Radian(0.1f), 20f);

            #endregion

            #region Light

            Light mainLight = scnMgr.createLight("MainLight");
            SceneNode lightNode = scnMgr.getRootSceneNode().createChildSceneNode();
            lightNode.setPosition(0, 10, 15);
            lightNode.attachObject(mainLight);

            #endregion

            #region Entity

            Entity entity = scnMgr.createEntity("Sinbad.mesh");
            SceneNode entityNode = scnMgr.getRootSceneNode().createChildSceneNode();
            entityNode.setPosition(0, -2.3f, 0);
            entityNode.attachObject(entity);            

            #endregion
        }

        private void OgreHeadScene()
        {
            #region Ambient Light

            scnMgr.setAmbientLight(new ColourValue(0.2f, 0.2f, 0.2f));

            #endregion

            #region Camera

            SceneNode camNode = scnMgr.getRootSceneNode().createChildSceneNode();
            cam = scnMgr.createCamera("myCam");
            cam.setNearClipDistance(5);
            cam.setAutoAspectRatio(true);
            camNode.setPosition(0, 47, 222);
            camNode.attachObject(cam);

            #endregion

            #region Light

            Light mainLight = scnMgr.createLight("MainLight");
            SceneNode lightNode = scnMgr.getRootSceneNode().createChildSceneNode();
            lightNode.setPosition(20, 80, 50);
            lightNode.attachObject(mainLight);

            #endregion

            #region Entity

            Entity entity = scnMgr.createEntity("ogrehead.mesh");
            SceneNode entityNode = scnMgr.getRootSceneNode().createChildSceneNode();
            entityNode.attachObject(entity);

            Entity entity2 = scnMgr.createEntity("ogrehead.mesh");
            SceneNode entityNode2 = scnMgr.getRootSceneNode().createChildSceneNode(new Vector3(84, 48, 0));
            entityNode2.attachObject(entity2);

            Entity entity3 = scnMgr.createEntity("ogrehead.mesh");
            SceneNode entityNode3 = scnMgr.getRootSceneNode().createChildSceneNode(new Vector3(0, 104, 0));
            entityNode3.setScale(2, 1.2f, 2);
            entityNode3.attachObject(entity3);

            Entity entity4 = scnMgr.createEntity("ogrehead.mesh");
            SceneNode entityNode4 = scnMgr.getRootSceneNode().createChildSceneNode(new Vector3(-84, 48, 0));
            entityNode4.roll(new Radian(new Degree(-90).valueRadians()));
            entityNode4.attachObject(entity4);

            #endregion
        }
    }
}
