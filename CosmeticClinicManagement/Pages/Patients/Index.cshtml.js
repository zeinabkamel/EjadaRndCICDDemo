$(function () {
    var l = abp.localization.getResource('CosmeticClinicManagement');

    var editModal = new abp.ModalManager(abp.appPath + 'Patients/Edit');
    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    var dataTable = $('#PatientsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[0, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(
                cosmeticClinicManagement.services.implementation.patient.getList
            ),
            columnDefs: [
                {
                    title: l('FirstName'),
                    data: "firstName"
                },
                {
                    title: l('LastName'),
                    data: "lastName"
                },
                {
                    title: l('DateOfBirth'),
                    data: "dateOfBirth",
                    dataFormat: "date"
                },
                {
                    title: l('PhoneNumber'),
                    data: "phoneNumber",
                    className: "text-start"
                },
                {
                    title: l('Email'),
                    data: "email"
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('PatientDeletionConfirmationMessage', data.record.firstName + ' ' + data.record.lastName);
                                },
                                action: function (data) {
                                    cosmeticClinicManagement.services.implementation.patient.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                }
            ]
        })
    );

    var createModal = new abp.ModalManager(abp.appPath + 'Patients/Create');
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewPatientButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});