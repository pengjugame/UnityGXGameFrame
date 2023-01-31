using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrame
{
    public abstract class Entity : IReference
    {
        public enum EntityStatus : byte
        {
            None = 0,
            IsCreated = 1 << 1,
            IsClear = 1 << 2,
        }

        protected Entity ComponentParent;

        private Dictionary<Type, Entity> m_Components;

        private EntityStatus m_EntityStatus;

        public int ID { get; private set; }

        private static int m_SerialId;

        protected Entity()
        {
            m_Components = new Dictionary<Type, Entity>();
            ID = ++m_SerialId;
        }

        private Entity Create<T>() where T : Entity
        {
            Type type = typeof(T);
            Entity entity = ReferencePool.Acquire(type) as Entity;
            entity.m_EntityStatus = EntityStatus.IsCreated;
            entity.ComponentParent = this;
            m_Components.Add(type, entity);
            entity.InitializeSystem();
            EnitityHouse.Instance.AddEntity(entity);
            return entity;
        }

        private void Remove<T>() where T : Entity
        {
            Type type = typeof(T);
            if (!m_Components.TryGetValue(type, out Entity entity))
            {
                throw new Exception($"entity not already  component: {type.FullName}");
            }

            entity.m_EntityStatus = EntityStatus.IsClear;
            m_Components.Remove(type);
            EnitityHouse.Instance.RemoveEntity(entity);
            ReferencePool.Release(entity);
        }

        /// <summary>
        /// 加入entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T AddComponent<T>() where T : Entity
        {
            Type type = typeof(T);
            if (this.m_Components != null && this.m_Components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }
            Entity component = Create<T>();
            return component as T;
        }

        /// <summary>
        /// 删除组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveComponent<T>() where T : Entity
        {
            Remove<T>();
        }
        
        /// <summary>
        /// 挂载实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddChild<T>() where T : Entity
        {
            Type type = typeof(T);
            Entity component = Create<T>();
            return component as T;
        }
        
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void RemoveChild<T>() where T : Entity
        {
            Remove<T>();
        }

        public abstract void InitializeSystem();
        /// <summary>
        /// 清除
        /// </summary>
        public virtual void Clear()
        {
            foreach (var components in m_Components)
            {
                components.Value.Clear();
            }
            m_Components.Clear();
        }
    }
}