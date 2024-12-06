using ActCourse.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ActCourse.Backend.Data
{
    public class CourseData : ICourse
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CourseData(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Course> Add(Course entity)
        {
            try
            {
                _applicationDbContext.Courses.Add(entity);
                await _applicationDbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding course: {ex.Message}");
            }
        }

        public async Task<Course> Delete(int id)
        {
            try
            {
                var course = await GetById(id);
                if (course == null)
                {
                    throw new Exception("Course not found");
                }
                _applicationDbContext.Courses.Remove(course);
                await _applicationDbContext.SaveChangesAsync();
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting course: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            var courses = await _applicationDbContext.Courses
                .Include(c => c.Category) // Include related Category data
                .ToListAsync();
            return courses;
        }

        public async Task<Course> GetById(int id)
        {
            var course = await _applicationDbContext.Courses
                .Include(c => c.Category) // Include related Category data
                .FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            return course;
        }

        public async Task<Course> Update(Course entity)
        {
            try
            {
                var course = await GetById(entity.CourseId);
                if (course == null)
                {
                    throw new Exception("Course not found");
                }
                course.Name = entity.Name;
                course.ImageName = entity.ImageName;
                course.Duration = entity.Duration;
                course.Description = entity.Description;
                course.CategoryId = entity.CategoryId; // Update the CategoryId if needed
                await _applicationDbContext.SaveChangesAsync();
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating course: {ex.Message}");
            }
        }
    }
}