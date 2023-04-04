import.meta.glob(['../fonts/**']);
import '../scss/site.scss';
import Alpine from 'alpinejs';
import Swal from 'sweetalert2';
window.Swal = Swal;
window.Alpine = Alpine;
Alpine.start();

document.querySelectorAll('form.destroy').forEach((form) => {
    form.addEventListener('submit', (event) => {
        event.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(event.target.action, {
                    method: 'POST',
                    body: new FormData(event.target),
                }).then(() => {
                    Swal.fire(
                        'Deleted!',
                        'Your record has been deleted.',
                        'success',
                    ).then((result) => {
                        if (result.isConfirmed) {
                            location.reload();
                        }
                    });
                });
            }
        });
    });
});