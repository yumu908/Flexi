using NUnit.Framework;
using UnityEngine;

namespace Physalia.Stats.Tests
{
    public class StatOwnerRepositoryTests
    {
        [Test]
        public void CreateOwner_TheCreatedOwnerIsManagedByRepository()
        {
            StatDefinitionTable table = new StatDefinitionTable.Factory().Create(StatTestHelper.ValidList);
            var repository = new StatOwnerRepository(table);
            StatOwner owner = repository.CreateOwner();

            Assert.AreSame(owner, repository.GetOwner(owner.Id));
        }

        [Test]
        public void CreateOwner_IsValidReturnsTrue()
        {
            StatDefinitionTable table = new StatDefinitionTable.Factory().Create(StatTestHelper.ValidList);
            var repository = new StatOwnerRepository(table);
            StatOwner owner = repository.CreateOwner();

            Assert.AreEqual(true, owner.IsValid());
        }

        [Test]
        public void DestroyOwner_IsValidReturnsFalse()
        {
            StatDefinitionTable table = new StatDefinitionTable.Factory().Create(StatTestHelper.ValidList);
            var repository = new StatOwnerRepository(table);
            StatOwner owner = repository.CreateOwner();

            owner.Destroy();

            Assert.AreEqual(false, owner.IsValid());
        }

        [Test]
        public void RemoveOwner_TheOwnerIsNull_LogError()
        {
            StatDefinitionTable table = new StatDefinitionTable.Factory().Create(StatTestHelper.ValidList);
            var repository = new StatOwnerRepository(table);

            repository.RemoveOwner(null);

            StatTestHelper.LogAssert(LogType.Error);
        }

        [Test]
        public void RemoveOwner_TheOwnerDoesNotBelongToTargetRepository_LogError()
        {
            StatDefinitionTable table = new StatDefinitionTable.Factory().Create(StatTestHelper.ValidList);
            var repository1 = new StatOwnerRepository(table);
            var repository2 = new StatOwnerRepository(table);
            StatOwner ownerFrom1 = repository1.CreateOwner();

            repository2.RemoveOwner(ownerFrom1);

            StatTestHelper.LogAssert(LogType.Error);
        }

        [Test]
        public void RemoveOwner_Success_GetOwnerWithTheSameIdReturnsNull()
        {
            StatDefinitionTable table = new StatDefinitionTable.Factory().Create(StatTestHelper.ValidList);
            var repository = new StatOwnerRepository(table);
            StatOwner owner = repository.CreateOwner();

            repository.RemoveOwner(owner);

            Assert.IsNull(repository.GetOwner(owner.Id));
        }
    }
}