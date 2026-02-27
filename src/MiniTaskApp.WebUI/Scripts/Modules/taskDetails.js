var taskId = window.location.pathname.split("/").pop();

function loadTask() {
    $.ajax({
        url: apiBaseUrl.tasks + taskId,
        type: "GET",
        success: function (t) {
            $("#taskHeader").html(
                `<h3>${t.title} (${t.status ?? ""})</h3>`
            );
        },
        error: function (err) {
            console.error("Error loading task:", err);
            Swal.fire('Error', 'Unable to load task.', 'error');
        }
    });
}

function loadItems() {
    $.ajax({
        url: apiBaseUrl.tasks + taskId,
        type: "GET",
        success: function (t) {
            let rows = "";
            $.each(t.items ?? [], function (i, item) {
                rows += `
                    <tr>
                        <td>${item.itemName}</td>
                        <td>${getTaskItemStatusText(item.status)}</td>
                        <td>${item.employeeName ?? ""}</td>
                        <td>
                            <button class="btn btn-warning btn-sm"
                                onclick="editItem(${item.taskItemId})">
                                Edit
                            </button>
                            <button class="btn btn-danger btn-sm"
                                onclick="deleteItem(${item.taskItemId})">
                                Delete
                            </button>
                        </td>
                    </tr>`;
            });
            $("#itemsTable tbody").html(rows);
        },
        error: function (err) {
            console.error("Error loading items:", err);
            Swal.fire('Error', 'Unable to load items.', 'error');
        }
    });
}

function loadEmployees() {
    $.ajax({
        url: apiBaseUrl.employees,
        type: "GET",
        success: function (emps) {
            let opts = `<option value="">-- Select Employee --</option>`;
            $.each(emps, function (i, e) {
                opts += `<option value="${e.employeeId}">${e.firstName} ${e.lastName}</option>`;
            });
            $("#employeeSelect").html(opts);
        },
        error: function (err) {
            console.error("Error loading employees:", err);
            Swal.fire('Error', 'Unable to load employees.', 'error');
        }
    });
}

function saveItem() {
    const id = $("#itemId").val();
    const data = {
        itemName: $("#itemName").val(),
        status: $("#itemStatus").val(),
        employeeId: $("#employeeSelect").val() ? parseInt($("#employeeSelect").val()) : null
    };
    const method = id ? "PUT" : "POST";
    const url = id ? `${apiBaseUrl.taskItems}${id}` : `${apiBaseUrl.tasks}${taskId}/items`;

    $.ajax({
        url: url,
        type: method,
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function () {
            $("#itemModal").modal("hide");
            clearItemForm();
            loadItems();
            loadTask();
            Swal.fire('Success', 'Item saved successfully!', 'success');
        },
        error: function (xhr) {
            if (xhr.status === 409) {
                Swal.fire('Warning', xhr.responseJSON.message, 'warning');
            } else if (xhr.status === 400) {
                Swal.fire('Warning', xhr.responseJSON.message, 'warning');
            } else {
                Swal.fire('Error', 'Unable to save task item.', 'error');
            }
        }
    });
}

function editItem(id) {
    $.ajax({
        url: `${apiBaseUrl.taskItems}${id}`,
        type: "GET",
        success: function (i) {
            $("#itemId").val(i.taskItemId);
            $("#itemName").val(i.itemName);
            $("#itemStatus").val(i.status);
            $("#employeeSelect").val(i.employeeId);
            $("#itemModal").modal("show");
        },
        error: function (err) {
            console.error("Error loading item:", err);
            Swal.fire('Error', 'Unable to load item.', 'error');
        }
    });
}

function deleteItem(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This item will be permanently deleted!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${apiBaseUrl.taskItems}${id}`,
                type: "DELETE",
                success: function () {
                    loadItems();
                    loadTask();
                    Swal.fire('Deleted!', 'Item has been deleted.', 'success');
                },
                error: function (err) {
                    console.error("Error deleting item:", err);
                    Swal.fire('Error', 'Unable to delete item.', 'error');
                }
            });
        }
    });
}

function clearItemForm() {
    $("#itemId").val("");
    $("#itemName").val("");
    $("#itemStatus").val("");
    $("#employeeSelect").val("");
}

function getTaskItemStatusText(status) {
    switch (parseInt(status)) {
        case 0:
            return "New";
        case 1:
            return "In Progress";
        case 2:
            return "Done";
        default:
            return "New";
    }
}