function loadTasks() {
    $.ajax({
        url: apiBaseUrl.tasks,
        type: "GET",
        success: function (data) {
            let rows = "";
            $.each(data, function (i, t) {
                rows += `
                    <tr>
                        <td>${t.title ?? ""}</td>
                        <td>${t.description ?? ""}</td>
                        <td>${t.status ?? ""}</td>
                        <td>
                            <a href="/Tasks/Details/${t.taskId}" 
                               class="btn btn-info btn-sm">View</a>
                            <button class="btn btn-warning btn-sm"
                                    onclick="editTask(${t.taskId})">
                                    Edit
                            </button>
                            <button class="btn btn-danger btn-sm"
                                    onclick="deleteTask(${t.taskId})">
                                    Delete
                            </button>
                        </td>
                    </tr>`;
            });
            $("#tasksTable tbody").html(rows);
        },
        error: function (err) {
            console.error("Error loading tasks:", err);
            Swal.fire('Error', 'Unable to load tasks.', 'error');
        }
    });
}
function saveTask() {
    const title = $("#taskTitle").val().trim();
    const description = $("#taskDescription").val().trim();

    if (!title || !description) {
        Swal.fire('Warning', 'Please fill in all fields before saving.', 'warning');
        return;
    }

    const id = $("#taskId").val();
    const task = { title, description };
    const method = id ? "PUT" : "POST";
    const url = id ? `${apiBaseUrl.tasks}${id}` : `${apiBaseUrl.tasks}`;

    $.ajax({
        url: url,
        type: method,
        contentType: "application/json",
        data: JSON.stringify(task),
        success: function () {
            $("#taskModal").modal("hide");
            clearTaskForm();
            loadTasks();
            Swal.fire('Success', 'Task saved successfully!', 'success');
        },
        error: function (xhr) {
            if (xhr.status === 409) {
                Swal.fire('Warning', xhr.responseJSON.message, 'warning');
            } else {
                Swal.fire('Error', 'Unable to save task.', 'error');
            }
        }
    });
}

function editTask(id) {
    $.ajax({
        url: `${apiBaseUrl.tasks}${id}`,
        type: "GET",
        success: function (t) {
            $("#taskId").val(t.taskId);
            $("#taskTitle").val(t.title);
            $("#taskDescription").val(t.description);

            $("#taskModalTitle").text("Edit Task");
            $("#taskModal").modal("show");
        },
        error: function (err) {
            console.error("Error loading task:", err);
            Swal.fire('Error', 'Unable to load task.', 'error');
        }
    });
}

function deleteTask(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This task will be permanently deleted!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${apiBaseUrl.tasks}${id}`,
                type: "DELETE",
                success: function () {
                    loadTasks();
                    Swal.fire('Deleted!', 'Task has been deleted.', 'success');
                },
                error: function (err) {
                    console.error("Error deleting task:", err);
                    Swal.fire('Error', 'Unable to delete task.', 'error');
                }
            });
        }
    });
}

function clearTaskForm() {
    $("#taskId").val("");
    $("#taskTitle").val("");
    $("#taskDescription").val("");
}