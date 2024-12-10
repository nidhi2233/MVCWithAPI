using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class TaskRepository : TaskInterface
    {
        private readonly NpgsqlConnection _conn;

        public TaskRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }
        public async Task<ResponseModel<string>> AddTask(TaskModel task)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();
                using (NpgsqlCommand cm = new NpgsqlCommand(@"INSERT INTO public.t_tasks (c_projectid, c_empid, c_title, c_description, c_estimateddays, c_startdate, c_enddate, c_status) VALUES (@c_projectid, @c_empid, @c_title, @c_description, @c_estimateddays, @c_startdate, @c_enddate, @c_status);", _conn))
                {
                    cm.Parameters.AddWithValue("c_projectid", task.ProjectId);
                    cm.Parameters.AddWithValue("c_empid", task.EmpId);
                    cm.Parameters.AddWithValue("@c_title", task.Title);
                    cm.Parameters.AddWithValue("@c_description", task.Description);
                    cm.Parameters.AddWithValue("@c_estimateddays", task.EstimatedDays);
                    cm.Parameters.AddWithValue("@c_startdate", task.StartDate);
                    cm.Parameters.AddWithValue("@c_enddate", task.EndDate);
                    cm.Parameters.AddWithValue("@c_status", task.Status);

                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Task Inserted Succsessfully.";
                    }
                    else
                    {
                        response.success = false;
                        response.message = "Error occurs while Insertion.";
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }

        public async Task<ResponseModel<string>> DeleteTask(int id)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"DELETE FROM public.t_tasks WHERE c_taskid=@c_taskid;", _conn))
                {
                    cm.Parameters.AddWithValue("c_taskid", id);
                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Task Deleted Succsessfully.";
                    }
                    else
                    {
                        response.success = false;
                        response.message = "Error occurs while Deletion.";
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }

        public string CalculateTaskAccuracy(TaskModel task)
        {
            try
            {
                // Calculate actual duration
                TimeSpan actualDuration = task.EndDate - task.StartDate;
                int actualDays = actualDuration.Days;

                // Calculate difference between actual and estimated days
                int difference = actualDays - task.EstimatedDays;

                // Determine performance evaluation
                if (difference == 0)
                {
                    return "Task was completed on time.";
                }
                else if (difference < 0)
                {
                    return $"Task was completed early by {Math.Abs(difference)} day(s).";
                }
                else
                {
                    return $"Task was delayed by {difference} day(s).";
                }
            }
            catch (Exception ex)
            {
                return $"Error calculating task accuracy: {ex.Message}";
            }
        }

        public async Task<ResponseModel<List<TaskModel>>> GetAllTaskAndById(int TaskId = 0, int EmpId = 0, int ProjectId = 0)
        {
            ResponseModel<List<TaskModel>> response = new ResponseModel<List<TaskModel>>();
            List<TaskModel> listTask = new List<TaskModel>();
            try
            {
                await _conn.OpenAsync();
                string query = null!;
                if (TaskId != 0)
                {
                    query = @"SELECT task.c_taskid, task.c_projectid,project.c_projectname, task.c_empid,emp.c_name, task.c_title, task.c_description, task.c_estimateddays, 
                    task.c_startdate, task.c_enddate, task.c_status FROM public.t_tasks as task 
                    JOIN public.t_project as project ON project.c_projectid = task.c_projectid
                    JOIN public.t_employee as emp ON emp.c_empid = task.c_empid WHERE task.c_taskid = @c_taskid;";
                }
                else if (EmpId != 0)
                {
                    query = @"SELECT task.c_taskid, task.c_projectid,project.c_projectname, task.c_empid,emp.c_name, task.c_title, task.c_description, task.c_estimateddays, 
                    task.c_startdate, task.c_enddate, task.c_status FROM public.t_tasks as task 
                    JOIN public.t_project as project ON project.c_projectid = task.c_projectid
                    JOIN public.t_employee as emp ON emp.c_empid = task.c_empid WHERE task.c_empid = @c_empid;";
                }
                else if (ProjectId != 0)
                {
                    query = @"SELECT task.c_taskid, task.c_projectid,project.c_projectname, task.c_empid,emp.c_name, task.c_title, task.c_description, task.c_estimateddays, 
                    task.c_startdate, task.c_enddate, task.c_status FROM public.t_tasks as task 
                    JOIN public.t_project as project ON project.c_projectid = task.c_projectid
                    JOIN public.t_employee as emp ON emp.c_empid = task.c_empid WHERE task.c_projectid = @c_projectid;";
                }
                else
                {
                    response.message = "No records found";
                    response.success = false;
                }
                using (NpgsqlCommand command = new NpgsqlCommand(query, _conn))
                {
                    command.Parameters.AddWithValue("c_taskid", TaskId);
                    command.Parameters.AddWithValue("c_projectid", ProjectId);
                    command.Parameters.AddWithValue("c_empid", EmpId);
                    NpgsqlDataReader dataReader = await command.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            listTask.Add(new TaskModel
                            {
                                TaskId = Convert.ToInt32(dataReader["c_taskid"]),
                                ProjectId = Convert.ToInt32(dataReader["c_projectid"]),
                                EmpId = Convert.ToInt32(dataReader["c_empid"]),
                                Title = dataReader["c_title"].ToString()!,
                                Description = dataReader["c_description"].ToString()!,
                                EstimatedDays = Convert.ToInt32(dataReader["c_estimateddays"]),
                                StartDate = Convert.ToDateTime(dataReader["c_startdate"]),
                                EndDate = Convert.ToDateTime(dataReader["c_enddate"]),
                                Status = dataReader["c_status"].ToString()!,
                                project = new Project{
                                    ProjectName = dataReader["c_projectname"].ToString()!
                                },
                                employee = new Employee{
                                    EmpName = dataReader["c_name"].ToString()!
                                }
                            });
                        }
                        response.message = "";
                        response.success = true;
                        response.data = listTask;
                    }
                    else
                    {
                        response.message = "No records found";
                        response.success = false;
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }

        public async Task<ResponseModel<TaskModel>> GetTaskById(int id)
        {
            ResponseModel<TaskModel> response = new ResponseModel<TaskModel>();
            TaskModel Task = new TaskModel();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"SELECT c_taskid, c_projectid, c_empid, c_title, c_description, c_estimateddays, c_startdate, c_enddate, c_status FROM public.t_tasks WHERE c_taskid=@c_taskid;", _conn))
                {
                    cm.Parameters.AddWithValue("c_taskid", id);
                    NpgsqlDataReader dataReader = await cm.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {

                            Task.TaskId = Convert.ToInt32(dataReader["c_taskid"]);
                            Task.ProjectId = Convert.ToInt32(dataReader["c_projectid"]);
                            Task.EmpId = Convert.ToInt32(dataReader["c_empid"]);
                            Task.Title = dataReader["c_title"].ToString()!;
                            Task.Description = dataReader["c_description"].ToString()!;
                            Task.EstimatedDays = Convert.ToInt32(dataReader["c_estimateddays"]);
                            Task.StartDate = Convert.ToDateTime(dataReader["c_startdate"]);
                            Task.EndDate = Convert.ToDateTime(dataReader["c_enddate"]);
                            Task.Status = dataReader["c_status"].ToString()!;
                        }

                        // Add task performance evaluation
                        Task.Status = $"{Task.Status} | {CalculateTaskAccuracy(Task)}";

                        response.message = "";
                        response.success = true;
                        response.data = Task;


                    }
                    else
                    {
                        response.message = "No record found";
                        response.success = false;
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }

        public async Task<ResponseModel<string>> UpdateTask(TaskModel task)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();
                using (NpgsqlCommand cm = new NpgsqlCommand(@"UPDATE public.t_tasks SET c_projectid=@c_projectid, c_empid=@c_empid, c_title=@c_title, c_description=@c_description, c_estimateddays=@c_estimateddays, c_startdate=@c_startdate, c_enddate=@c_enddate, c_status=@c_status WHERE c_taskid=@c_taskid;", _conn))
                {
                    cm.Parameters.AddWithValue("@c_taskid", task.TaskId);
                    cm.Parameters.AddWithValue("@c_projectid", task.ProjectId);
                    cm.Parameters.AddWithValue("@c_empid", task.EmpId);
                    cm.Parameters.AddWithValue("@c_title", task.Title);
                    cm.Parameters.AddWithValue("@c_description", task.Description);
                    cm.Parameters.AddWithValue("@c_estimateddays", task.EstimatedDays);
                    cm.Parameters.AddWithValue("@c_startdate", task.StartDate);
                    cm.Parameters.AddWithValue("@c_enddate", task.EndDate);
                    cm.Parameters.AddWithValue("@c_status", task.Status);


                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Task Updated Succsessfully.";
                    }
                    else
                    {
                        response.success = false;
                        response.message = "Somthing Went Wrong Here.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }
    }

}