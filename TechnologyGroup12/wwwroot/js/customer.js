var dataTable;

$(document).ready(function () {
    loadDataTable();
    
    
});

function loadDataTable() {

    dataTable = $("#tblData").DataTable(
        {
            "bPaginate": false,
            "bFilter": false,
            "bInfo": false,
            "ajax": {
                "url": "/Customer/GetAll"
            },
            "columns": [
                { "data": "name", "width": "15%" },
                { "data": "phone", "width": "15%" },
                { "data": "email", "width": "15%" },
                { "data": "address", "width": "30%" },
                {
                    "data": {
                        isVip: "isVip"
                    },
                    "render": function (data) {

                        if (data.isVip == false) {
                            return `
                            <div class="text-center">
                                <i class="btn btn-warning text-white fas fa-check"></i> 
                            </div>
                            `;
                        }
                        else {
                            return `
                            <div class="text-center">
                                <i class="btn btn-danger text-white fas fa-ban"></i> 
                            </div>
                            `;
                        }
                    }, "width": "10%"
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="text-center">
                                <a href="/Customer/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Customer/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                            `;
                    }, "width": "10%"
                }
            ]
        })
}

const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(url) {
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        $('#tblData').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'Can not delete this, maybe it not exit or error from sever',
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}