using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ResourceManagerTest {

	[Test]
	public void NewPlayModeTestSimplePasses() {
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewPlayModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		ResourceManager.Instance.LoadAsync(ResourceID.audio_boss1, (asset) =>
        {
			Debug.Log(asset.name);
			Assert.AreEqual(ResourceManager.Instance.GetAsset(ResourceID.audio_boss1), asset);
        });
        ResourceManager.Instance.LoadAsync(ResourceID.audio_boss2, (asset) =>
        {
			Debug.Log(asset.name);
			Assert.AreEqual(ResourceManager.Instance.GetAsset(ResourceID.audio_boss2), asset);
        });
        ResourceManager.Instance.LoadAsync(ResourceID.audio_boss3, (asset) =>
        {
			Debug.Log(asset.name);
            Assert.AreEqual(ResourceManager.Instance.GetAsset(ResourceID.audio_boss3), asset);
        });
		yield return new WaitForSeconds(3f);
	}
}
