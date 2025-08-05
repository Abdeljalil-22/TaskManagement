CREATE VIEW ProjectsView AS
SELECT 
    p.Id,
    p.Name,
    p.Description,
    p.OwnerId,
    u.Name AS OwnerName,
    (SELECT COUNT(*) FROM Tasks t WHERE t.ProjectId = p.Id) AS TotalTasks,
    (SELECT COUNT(*) FROM Tasks t WHERE t.ProjectId = p.Id AND t.Status = 'Done') AS CompletedTasks,
    p.CreatedAt,
    p.LastUpdated
FROM 
    Projects p
    LEFT JOIN Users u ON p.OwnerId = u.Id;

CREATE VIEW TasksView AS
SELECT 
    t.Id,
    t.Title,
    t.Description,
    t.AssignedUserId,
    u.Name AS AssignedUserName,
    t.DueDate,
    t.Status,
    t.CreatedAt,
    t.LastUpdated
FROM 
    Tasks t
    LEFT JOIN Users u ON t.AssignedUserId = u.Id;
