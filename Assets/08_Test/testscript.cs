using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BlobbInvasion.Utilities;
using BlobbInvasion.Gameplay.Items;
using BlobbInvasion.UI;

namespace BlobbInvasion.Tests
{
    public class testscript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void testscriptSimplePasses()
        {
            var type = Utils.ConvertCollectableType(CollectableType.BUL_BIG);
            Assert.AreEqual(CraftingType.BULLET,type);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator testscriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
