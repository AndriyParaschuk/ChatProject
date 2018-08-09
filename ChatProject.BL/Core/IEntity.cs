using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
