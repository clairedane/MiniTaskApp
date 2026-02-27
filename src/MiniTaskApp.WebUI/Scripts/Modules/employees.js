function loadEmployees() {
    $.ajax({
        url: apiBaseUrl.employees,
        type: "GET",
        success: function (data) {
            let rows = "";
            $.each(data, function (i, e) {
                rows += `
                    <tr>
                        <td>${e.employeeNo}</td>
                        <td>${e.firstName} ${e.lastName}</td>
                        <td>${e.email}</td>
                        <td>${e.isActive ? "Active" : "Inactive"}</td>
                        <td>
                            <button class="btn btn-warning btn-sm"
                                onclick="editEmployee(${e.employeeId})">Edit</button>
                            <button class="btn btn-danger btn-sm"
                                onclick="deleteEmployee(${e.employeeId})">Delete</button>
                        </td>
                    </tr>`;
            });
            $("#employeesTable tbody").html(rows);
        },
        error: function (err) {
            console.error("Error loading employees:", err);
            Swal.fire('Error', 'Unable to load employees.', 'error');
        }
    });
}

function saveEmployee() {
    const employeeNo = $("#employeeNo").val().trim();
    const firstName = $("#firstName").val().trim();
    const lastName = $("#lastName").val().trim();
    const email = $("#email").val().trim();

    if (!employeeNo || !firstName || !lastName || !email) {
        Swal.fire('Warning', 'Please fill in all fields before saving.', 'warning');
        return;
    }

    const id = $("#employeeId").val();
    const data = {
        employeeId: id,
        employeeNo,
        firstName,
        lastName,
        email,
        isActive: $("#isActive").is(":checked")
    };

    const method = id ? "PUT" : "POST";
    const url = id ? `${apiBaseUrl.employees}${id}` : `${apiBaseUrl.employees}`;

    $.ajax({
        url: url,
        type: method,
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function () {
            $("#employeeModal").modal("hide");
            loadEmployees();
            Swal.fire('Success', 'Employee saved successfully!', 'success');
        },
        error: function (xhr) {
            if (xhr.status === 409) {
                Swal.fire('Warning', xhr.responseJSON.message, 'warning');
            } else if (xhr.status === 400) {
                Swal.fire('Warning', xhr.responseJSON.message, 'warning');
            } else {
                Swal.fire('Error', 'Unable to save employee.', 'error');
            }
        }
    });
}
function editEmployee(id) {
    $.ajax({
        url: `${apiBaseUrl.employees}${id}`,
        type: "GET",
        success: function (e) {
            $("#employeeId").val(e.employeeId);
            $("#employeeNo").val(e.employeeNo);
            $("#firstName").val(e.firstName);
            $("#lastName").val(e.lastName);
            $("#email").val(e.email);
            $("#isActive").prop("checked", e.isActive);

            $("#employeeModal").modal("show");
        },
        error: function (err) {
            console.error("Error loading employee:", err);
            Swal.fire('Error', 'Unable to load employee.', 'error');
        }
    });
}

function deleteEmployee(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This employee will be permanently deleted!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${apiBaseUrl.employees}${id}`,
                type: "DELETE",
                success: function () {
                    loadEmployees();
                    Swal.fire('Deleted!', 'Employee has been deleted.', 'success');
                },
                error: function (err) {
                    console.error("Error deleting employee:", err);
                    Swal.fire('Error', 'Unable to delete employee.', 'error');
                }
            });
        }
    });
}

function clearEmployeeForm() {
    $("#employeeId").val("");
    $("#employeeNo").val("");
    $("#firstName").val("");
    $("#lastName").val("");
    $("#email").val("");
    $("#isActive").prop("checked", true);
}