using ClockApplicationBO.Models;
using ClockApplicationDAO;
using ClockApplicationRepo.Interface;
using System.Linq.Expressions;

namespace ClockApplicationRepo;

public class AlarmRepo : IAlarmRepo
{
    private readonly AlarmDAO alarmDAO;
    public AlarmRepo()
    {
        alarmDAO = new AlarmDAO();
    }
    public void Create(Alarm entity)
    {
        alarmDAO.Create(entity);
    }

    public List<Alarm> GetAll()
    {
        return alarmDAO.GetAll();
    }

    public List<Alarm> GetAllWithInclude(params Expression<Func<Alarm, object>>[] includeProperties)
    {
        return alarmDAO.GetAllWithInclude(includeProperties);
    }

    public List<Alarm> GetAllWithPaging(int pageNum, int pageSize)
    {
        return alarmDAO.GetAllWithPaging(pageNum, pageSize);
    }

    public Alarm? GetById(int id)
    {
        return alarmDAO.GetById(id);
    }

    public bool Remove(Alarm entity)
    {
        return alarmDAO.Remove(entity);
    }

    public void Update(Alarm entity)
    {
        alarmDAO.Update(entity);
    }
}
