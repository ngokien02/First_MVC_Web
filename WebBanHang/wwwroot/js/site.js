// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(() => {
    //particles
    particlesJS.load('particles-js', './particles.json', function () {
        console.log('callback - particles.js config loaded');
    });

    //showmore button
    const take = 6;
    let skip = 0;
    var productCount = $("#productCount").val();
    var divCount = 0;

    function loadMore() {
        $.get("/Home/LoadMore", { skip: skip, take: take }, function (data) {
            $(".productList").append(data);
            skip += take;
            divCount = $(".productList .productCard").length;
            if (divCount == productCount) {
                $(".showMoreBtn").hide();
            }
        });
    };

    loadMore();
    $(".showMoreBtn").on("click", function () {
        loadMore();
    });


    //xu ly active
    $(document).on('click', '.nav-link', function (e) {
        $('.nav-link').removeClass('active');
        $(this).addClass('active');
    });

    $(document).on('click', '.categoryList', function (e) {
        $('.categoryList').removeClass('active');
        $(this).addClass('active');
    });

    //bien toan cuc xu ly load trang
    var urlSP = "/admin/product?page=1";
    var urlTL = "/admin/category";
    let loadPage = (url) => {
        $.get(url, function (data) {
            $(".contents").fadeOut(100, function () {
                $(".contents").html(data).fadeIn(100);
            });
        });
    }

    //xu ly them san pham
    document.getElementById("addProductForm").addEventListener("submit", function (e) {

        e.preventDefault();
        var formData = new FormData(this);

        $.ajax({
            method: "POST",
            url: "/admin/Product/Add",
            data: formData,

            contentType: false,
            processData: false,

            success: function (data) {
                if (data) {
                    $("p#thongBaoSP").text("Thêm 1 sản phẩm thành công!");
                    loadPage(urlSP);
                    $("#addProductForm")[0].reset();
                }
                else {
                    $("p#thongBaoSP").text("Thêm sản phẩm thất bại, vui lòng kiểm tra lại thông tin!");
                }
            }
        });
    });

    //xu ly them the loai
    document.getElementById("addCategoryForm").addEventListener("submit", function (e) {

        e.preventDefault();
        var formData = new FormData(this);

        $.ajax({
            method: "POST",
            url: "/admin/Category/Add",
            data: formData,

            contentType: false,
            processData: false,

            success: function (data) {
                if (data) {
                    $("p#thongBaoTL").text("Thêm 1 thể loại thành công!");
                    loadPage(urlTL);
                    $("#addCategoryForm")[0].reset();
                }
                else {
                    $("p#thongBaoTL").text("Thêm thể loại thất bại, vui lòng kiểm tra lại thông tin!");
                }
            }
        });
    });

    //xu ly modal
    $('#addProductModal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus');
    });

    $('#addCategoryModal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus');
    });

    //xu ly phan trang san pham
    $(document).on("click", "a.page-link, a.pagedProducts", function (e) {
        e.preventDefault();
        let pageUrl = $(this).attr("href");

        if (pageUrl) {
            $(".contents").fadeOut(100, function () {
                loadPage(pageUrl);
            });
        }
    });

    //xu ly ajax the loai
    $(document).on("click", ".categories", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");

        $.get(url, function (data) {
            $(".contents").fadeOut(100, function () {
                $(".contents").html(data).fadeIn(100);
            })
        })
    });

    //xu ly xoa san pham
    $(".contents").on("click", "button.btnDeleteSP", function () {
        var id = $(this).attr("Id");
        var _this = $(this);
        Swal.fire({
            title: "Xác nhận xóa",
            text: "Bạn có chắc muốn xóa sản phẩm này?",
            icon: "warning",
            showCancelButton: true,
            cancelButtonText: "Hủy",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Xóa"
        }).then((result) => {
            if (result.isConfirmed) {
                $.post("/admin/Product/DeleteConfirm", { Id: id }, function (data) {
                    if (data) {
                        var rowDelete = _this.closest("tr");
                        rowDelete.remove();
                        $('#thongBaoXoaSP').html("Đã xóa thành công 1 sản phẩm");
                    }
                    else $('#thongBaoXoaSP').html("Xóa thất bại, vui lòng liên hệ đội ngũ hỗ trợ");;
                });
            }
        });
    });

    //xu ly xoa the loai
    $(document).on("click", "button.btnDeleteTL", function (e) {
        var id = $(this).attr("Id");
        var _this = $(this);
        Swal.fire({
            title: "Xác nhận xóa",
            text: "Bạn có chắc muốn xóa thể loại này?",
            icon: "warning",
            showCancelButton: true,
            cancelButtonText: "Hủy",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Xóa"
        }).then((result) => {
            if (result.isConfirmed) {
                $.post("/admin/Category/DeleteConfirm", { Id: id }, function (data) {
                    if (data) {
                        var rowDelete = _this.closest("tr");
                        rowDelete.remove();
                        $('#thongBaoXoaTL').html("Đã xóa thành công 1 thể loại");
                    }
                    else $('#thongBaoXoaTL').html("Đã có sản phẩm thuộc thể loại này, không thể xóa!");;
                });
            }
        });
    });


    //xu ly ajax trang shopping
    $("a.shopping").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $.get(url, function (data) {
            $(".contents").html(data);
            $("ul.categoriesList li:nth-child(2)").addClass("active");
        })
    })

    //xu ly chon the loai trang shopping
    $(document).on("click", ".categoryList", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $.get(url, function (data) {
            if (data.trim() === "") {
                $(".productByCatId").html("<h4>Hiện chưa có sản phẩm thuộc thể loại này. Chúng tôi sẽ cố gắng cập nhật sớm nhất.</h4>");
                return;
            }
            $(".productByCatId").html(data);
        })
    })
});

