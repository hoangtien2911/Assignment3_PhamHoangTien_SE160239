using ClockApplicationBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClockApplicationRepo.Interface;

public interface IAlarmRepo
{
    List<Alarm> GetAllWithPaging(int pageNum, int pageSize);

    List<Alarm> GetAllWithInclude(params Expression<Func<Alarm, object>>[] includeProperties);

    List<Alarm> GetAll();
    void Create(Alarm entity);
    void Update(Alarm entity);
    bool Remove(Alarm entity);
    Alarm? GetById(int id);
}
