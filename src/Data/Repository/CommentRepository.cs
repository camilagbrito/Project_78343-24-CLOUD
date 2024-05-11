﻿using Business.Interfaces;
using Business.Models;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(EcomDbContext context) : base(context)
        {
        }
    }
}