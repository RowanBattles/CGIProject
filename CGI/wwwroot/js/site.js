import.meta.glob(['../fonts/**']);
import '../scss/site.scss';
import Alpine from 'alpinejs';
import Swal from 'sweetalert2';
windows.swal = Swal;
window.Alpine = Alpine;
Alpine.start();
