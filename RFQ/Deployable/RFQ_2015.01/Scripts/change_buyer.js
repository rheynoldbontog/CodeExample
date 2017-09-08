﻿/* File Created: September 5, 2017 */

$(function () {
    $.ajaxSetup({ cache: false });
    //ModalClickBind();
    buyerModal.modalClickBind();
});

var buyerModal = (function () {
    "use strict"

    var myModalLink = "a[change-buyer-modal]";
    var myModal = '#buyerChangeModal';
    var myModalContent = '#buyerChangeModalContent';
    var myModalFormId = '#modal_form';
    
    function resizeModal() {

        $('body #buyerChangeModal.modal').css('width', '600px');
        $('body #buyerChangeModal.modal').css('margin-left', '-300px');
        $('form#modal_form.form-horizontal').css('margin-bottom', '0px');
    }

    var bindForm = function (dialog) {
        $('form', dialog).submit(function () {

            if ($(this).valid()) {

                var actionUrl = this.action;
                var methodType = this.method;
                var formData = $(this).serialize();

                swal({
                    title: 'Are you sure?',
                    text: "Buyer will be reassigned!",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#303F9F',
                    cancelButtonColor: '#f39c12',
                    confirmButtonText: 'Yes, reassign buyer!'
                }).then(function () {
                    
                    //AJAX CALL HERE...
                    $.ajax({
                        url: actionUrl,
                        type: methodType,
                        data: formData,
                        success: function (result) {
                            if (result.success) {
                                swal({
                                    title: 'Good job!',
                                    text: 'Buyer has been reassigned!',
                                    timer: 3000,
                                    type: 'success'
                                }).then(
                                function () {
                                    $(myModal).modal('hide');
                                    window.location.href = window.location.href;
                                },
                                // handling the promise rejection
                                function (dismiss) {
                                    $(myModal).modal('hide');
                                    window.location.href = window.location.href;
                                });

                            } else {
                                $(myModalContent).html(result);

                                resizeModal();
                                //Parse form...
                                parseFormForValidation(myModalFormId);
                                bindForm(dialog);
                            }
                        }
                    });
                }, function (dismiss) {

                });
            }
            return false;
        });
    };

    var modalClickBind = function () {

        $(myModalLink).on("click", function (e) {
            $(myModalContent).load(this.href, function () {
                $(myModal).modal({
                    /*backdrop: 'static',*/
                    keyboard: true
                }, 'show');

                resizeModal();
                //Parse form...
                parseFormForValidation(myModalFormId);
                bindForm(this);
            });

            return false;
        });

    };

    return {
        modalClickBind: modalClickBind,
        
    };

})();