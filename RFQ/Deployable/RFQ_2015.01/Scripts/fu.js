/* File Created: August 17, 2017 */
(function ($) {

    $.fn.fu = function (options) {
        // $(this) contains the selector (#containerId)

        var opts = $.extend({}, $.fn.fu.defaults, options);

        return this.each(function (index, el) {
            var $this = $(this), data = $this.data('control');
            
            $this.find(".fu_button").on('click', function (event) {

                event.stopPropagation();
                event.stopImmediatePropagation();
                                
                var parentIndex = $this.find("[name='fu_upload_index']").val();
                var lastIndex = parseInt($this.find("[name='fu_upload_count']").val());

                var createUrl = $this.find("[name='fu_create_url']").val() + '&parentIndex=' + parentIndex + '&lastChildIndex=' + lastIndex; ;

                $.ajax({
                    url: createUrl,
                    type: 'GET',
                    data: null,
                    cache: false,
                    success: function (result) {

                        $this.find(".fu_uploads_container").append(result);

                        initializeUploaders();

                        //Increment index...
                        $this.find("[name='fu_upload_count']").val(parseInt(lastIndex) + 1);
                    }
                });
            });

            var initializeUploaders = function () {
                //Initialize each uploader...
                $this.find(".fu_upload_item").each(function () {

                    var itemId = '#' + $(this).attr('id');
                    var uploaderId = "#" + $(this).find(".fu_upload_file_name").attr('id');
                    var deleterId = "#" + $(this).find(".fu_upload_file_deleter").attr('id');
                    var isDeletedId = "#" + $(this).find(".fu_upload_file_is_deleted").attr('id');
                    var fileUrlId = "#" + $(this).find(".fu_upload_file_url").attr('id');
                    var containerId = "#" + $(this).parent('div').attr('id');

                    //Initialize delete method...
                    $(deleterId).on('click', function () {
                        swal({
                            title: 'Are you sure?',
                            text: "Attachment will be marked as deleted and will no longer be visible!",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#303F9F',
                            cancelButtonColor: '#f39c12',
                            confirmButtonText: 'Yes, delete attachment!'
                        }).then(function () {
                            $(isDeletedId).val(true);
                            $(itemId).fadeOut();
                        }, function (dismiss) {

                        });

                    });

                    //Initialize fine uploader...
                    var quoteUploader = new qq.FineUploaderBasic({
                        button: $(this).find(".fu_upload_file_uploader")[0],
                        request: {
                            endpoint: $this.find("[name='fu_upload_url']").val()
                        },
                        validation: {
                            acceptFiles: "application/msword, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/vnd.openxmlformats-officedocument.wordprocessing‌​ml.document, application/vnd.openxmlformats-officedocument.spreadsheetml.‌​sheet, text/plain, application/pdf, image/*",
                            allowedExtensions: ['jpeg', 'jpg', 'gif', 'png', 'pdf', 'doc', 'docx', 'xls', 'xlsx', 'txt', 'ppt', 'pptx']
                            //sizeLimit: 204800 // 200 kB = 200 * 1024 bytes
                        },
                        callbacks: {
                            onComplete: function (id, fileName, responseJSON) {
                                if (responseJSON.success) {
                                    $(uploaderId).val(responseJSON.filename);
                                    $(fileUrlId).val(responseJSON.fileurl);
                                } else {
                                    swal('Ooops...', 'File upload failed!', 'error');
                                }
                            },
                            onError: function (id, name, errorReason, xhrOrXdr) {
                                swal('Ooops...', qq.format("Error on file number {} - {}.  Reason: {}", id, name, errorReason), 'error');
                            }

                        },
                        debug: true
                    });

                });
            };

            initializeUploaders();
        });
    };

} (jQuery));