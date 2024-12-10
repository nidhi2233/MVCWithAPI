using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class ProjectRepository : ProjectInterface
    {
        private readonly NpgsqlConnection _conn;

        public ProjectRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }
        public async Task<ResponseModel<string>> AddProject(Project project)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();
                using (NpgsqlCommand cm = new NpgsqlCommand(@"INSERT INTO public.t_project (c_projectname, c_description) VALUES ( @c_projectname, @c_description);", _conn))
                {
                    cm.Parameters.AddWithValue("@c_projectname", project.ProjectName);
                    cm.Parameters.AddWithValue("@c_description", project.Description);
                    
                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Project Inserted Succsessfully.";
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
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<string>> DeleteProject(int id)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"DELETE FROM public.t_project WHERE c_projectid=@c_projectid;", _conn))
                {
                    cm.Parameters.AddWithValue("c_projectid",id);
                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Project Record Deleted Succsessfully.";
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
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<List<Project>>> GetAllProject()
        {
            ResponseModel<List<Project>> response = new ResponseModel<List<Project>>();
            List<Project> listProject = new List<Project>();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"SELECT c_projectid, c_projectname, c_description FROM public.t_project;", _conn))
                {
                    NpgsqlDataReader dataReader = await cm.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            listProject.Add(new Project
                            {
                                ProjectId = Convert.ToInt32(dataReader["c_projectid"]),
                                ProjectName = dataReader["c_projectname"].ToString()!,
                                Description = dataReader["c_description"].ToString()!,
                            });
                            response.message = "";
                            response.success = true;
                            response.data = listProject;
                        }
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

        public async Task<ResponseModel<Project>> GetProjectById(int id)
        {
            ResponseModel<Project> response = new ResponseModel<Project>();
            Project project = new Project();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"SELECT c_projectid, c_projectname, c_description FROM public.t_project WHERE c_projectid=@c_projectid;", _conn))
                {
                    cm.Parameters.AddWithValue("c_projectid", id);
                    NpgsqlDataReader dataReader = await cm.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            
                            project.ProjectId = Convert.ToInt32(dataReader["c_projectid"]);
                            project.ProjectName = dataReader["c_projectname"].ToString()!;
                            project.Description = dataReader["c_description"].ToString()!;
                           
                        }
                            response.message = "";
                            response.success = true;
                            response.data=project;
                            

                    }
                    else
                    {
                        response.message = "No record found";
                        response.success=false;
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

        public async Task<ResponseModel<string>> UpdateProject(Project project)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();
                using (NpgsqlCommand cm = new NpgsqlCommand(@"UPDATE public.t_project SET c_projectname=@c_projectname, c_description=@c_description WHERE c_projectid=@c_projectid ;", _conn))
                {
                    cm.Parameters.AddWithValue("@c_projectid", project.ProjectId);
                    cm.Parameters.AddWithValue("@c_projectname", project.ProjectName);
                    cm.Parameters.AddWithValue("@c_description", project.Description);

                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Record Updated Succsessfully.";
                    }
                    else
                    {
                        response.success = false;
                        response.message = "Somthing Went Wrong Here.";
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }
        }
    }
}