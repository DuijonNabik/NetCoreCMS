﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccTagService : IBaseService<NccTag>
    {
        private readonly NccTagRepository _entityRepository;

        public NccTagService(NccTagRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccTag Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string>() { "Posts" });
        }

        public NccTag GetWithPost(string name)
        {
            long entityId = 0;
            var tag = _entityRepository.LoadAll(true, -1, name).FirstOrDefault();
            if (tag != null)
                entityId = tag.Id;

            return _entityRepository.Get(entityId, false, new List<string>() { "Posts", "Posts.Post", "Posts.Post.Author", "Posts.Post.PostDetails", "Posts.Post.Comments" });
        }

        public List<NccTag> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "Posts" });
        }

        public List<NccTag> LoadRecentTag(int count)
        {
            return _entityRepository.LoadRecentTag(count);
        }

        public NccTag Save(NccTag entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccTag Update(NccTag entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(entity, oldEntity);
                    _entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccTag copyFrom, NccTag copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Metadata = copyFrom.Metadata;
        }

        public List<NccTag> Search(string name, int count = 20)
        {
            return _entityRepository.Search(name, count);
        }
    }
}