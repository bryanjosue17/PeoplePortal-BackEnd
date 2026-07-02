-- Limpiar datos existentes (se usa CASCADE por si hay dependencias futuras, aunque actualmente no las haya)
TRUNCATE TABLE hr_requests, documents, announcements, benefits RESTART IDENTITY CASCADE;

-- Obtener IDs de empleados (ya insertados por el fix anterior)
DO $$ 
DECLARE
    admin_id text := '29e218c9-a768-491c-84ae-81cd0677eaaa';
    hr_id text := '59612e29-6b6c-4159-b970-be79df3c95a2';
    manager_id text := 'e8f232a5-a95a-45e1-b542-5240b3577094';
    user_id text := '2ec78c39-d293-4bdd-9ff0-2d9ce163a8fd';
BEGIN
    -------------------------------------------
    -- 1. Beneficios Reales (Catálogo Global)
    -------------------------------------------
    INSERT INTO benefits (id, name, description, type, is_active) VALUES
    (gen_random_uuid(), 'Seguro Médico EPS', 'Cobertura médica privada al 100% para el titular y 50% para dependientes.', 'Salud', true),
    (gen_random_uuid(), 'Plan Dental', 'Cobertura anual de tratamientos dentales preventivos en la red Sonrisas.', 'Salud', true),
    (gen_random_uuid(), 'Día libre por Cumpleaños', 'Disfruta de un día libre remunerado en el mes de tu cumpleaños.', 'Tiempo Libre', true),
    (gen_random_uuid(), 'Descuento SmartFit', '20% de descuento en la membresía Black de SmartFit a nivel nacional.', 'Deporte y Bienestar', true),
    (gen_random_uuid(), 'Alianza Universitaria', '15% de descuento en diplomados y posgrados en la Universidad de la Innovación.', 'Educación', true),
    (gen_random_uuid(), 'Teletrabajo Flexible', 'Opción de hacer Home Office 2 veces por semana previo acuerdo con tu líder.', 'Calidad de Vida', true),
    (gen_random_uuid(), 'Bono por Referido', 'Bono económico si un candidato que recomendaste pasa el periodo de prueba.', 'Financiero', true),
    (gen_random_uuid(), 'Chequeos Preventivos', 'Chequeo médico anual gratuito en clínicas afiliadas.', 'Salud', true);

    -------------------------------------------
    -- 2. Comunicados (Announcements)
    -------------------------------------------
    INSERT INTO announcements (id, title, body, type, published_at, created_by, is_active) VALUES
    (gen_random_uuid(), 'Nueva política de Teletrabajo', 'Estimado equipo, a partir del próximo mes implementaremos la política de 2 días de Home Office a la semana. Por favor coordinen con sus jefes directos los días asignados.', 'PolicyChange', NOW() - interval '2 days', admin_id, true),
    (gen_random_uuid(), 'Mantenimiento del Sistema', 'El día sábado a las 2:00 AM el sistema de nómina entrará en mantenimiento programado por 4 horas.', 'HrNotice', NOW() - interval '5 days', admin_id, true),
    (gen_random_uuid(), '¡Bienvenidos a los nuevos ingresos!', 'Queremos dar una cálida bienvenida a los 5 nuevos desarrolladores que se integran al equipo de Producto esta semana.', 'News', NOW() - interval '10 days', hr_id, true),
    (gen_random_uuid(), 'Fiesta de Fin de Año', '¡Guarden la fecha! La fiesta anual de la empresa será el 15 de Diciembre. Pronto enviaremos más detalles y el formulario de asistencia.', 'Event', NOW() - interval '12 days', hr_id, true),
    (gen_random_uuid(), 'Actualización de Datos Personales', 'Recordatorio: Tienen hasta fin de mes para actualizar su dirección y contacto de emergencia en el portal.', 'Reminder', NOW() - interval '1 day', hr_id, true);

    -------------------------------------------
    -- 3. Documentos (Archivos de Empleados)
    -------------------------------------------
    INSERT INTO documents (id, employee_id, name, type, status, file_url, uploaded_at) VALUES
    (gen_random_uuid(), user_id, 'Contrato Indeterminado.pdf', 'Contrato', 'Approved', 'https://example.com/docs/contrato1.pdf', NOW() - interval '180 days'),
    (gen_random_uuid(), user_id, 'DNI Frontal.png', 'DNI', 'Approved', 'https://example.com/docs/dni_user.png', NOW() - interval '180 days'),
    (gen_random_uuid(), user_id, 'Boleta Pago Mayo 2026.pdf', 'Boleta', 'Available', 'https://example.com/docs/boleta_mayo.pdf', NOW() - interval '30 days'),
    
    (gen_random_uuid(), manager_id, 'Acuerdo de Confidencialidad.pdf', 'Contrato', 'Approved', 'https://example.com/docs/nda_manager.pdf', NOW() - interval '400 days'),
    (gen_random_uuid(), manager_id, 'Certificado Liderazgo.pdf', 'Certificado', 'Pending', 'https://example.com/docs/cert_liderazgo.pdf', NOW() - interval '2 days'),
    
    (gen_random_uuid(), hr_id, 'Boleta Pago Junio 2026.pdf', 'Boleta', 'Available', 'https://example.com/docs/boleta_junio_hr.pdf', NOW() - interval '2 days'),
    (gen_random_uuid(), admin_id, 'Políticas TI Firmadas.pdf', 'Contrato', 'Approved', 'https://example.com/docs/politicas_ti.pdf', NOW() - interval '300 days');

    -------------------------------------------
    -- 4. Solicitudes de RRHH (HR Requests)
    -------------------------------------------
    -- user_id (Test User)
    INSERT INTO hr_requests (id, employee_id, type, status, reason, created_at_utc, vacation_start_date, vacation_end_date) VALUES
    (gen_random_uuid(), user_id, 'Vacation', 'Approved', 'Vacaciones anuales programadas con la familia.', NOW() - interval '60 days', CURRENT_DATE - interval '50 days', CURRENT_DATE - interval '40 days'),
    (gen_random_uuid(), user_id, 'Vacation', 'Submitted', 'Solicito 3 días a cuenta de vacaciones para trámites personales.', NOW() - interval '1 day', CURRENT_DATE + interval '15 days', CURRENT_DATE + interval '18 days'),
    (gen_random_uuid(), user_id, 'Certificate', 'Approved', 'Constancia de trabajo para solicitud de visado.', NOW() - interval '20 days', null, null);

    -- manager_id (Test Manager)
    INSERT INTO hr_requests (id, employee_id, type, status, reason, created_at_utc) VALUES
    (gen_random_uuid(), manager_id, 'Permission', 'Rejected', 'Permiso por mudanza.', NOW() - interval '10 days'),
    (gen_random_uuid(), manager_id, 'Voucher', 'Submitted', 'Adelanto de sueldo por emergencia médica.', NOW() - interval '3 hours');

    -- hr_id (Test HR)
    INSERT INTO hr_requests (id, employee_id, type, status, reason, created_at_utc, vacation_start_date, vacation_end_date) VALUES
    (gen_random_uuid(), hr_id, 'Vacation', 'InReview', 'Descanso vacacional anual.', NOW() - interval '5 days', CURRENT_DATE + interval '30 days', CURRENT_DATE + interval '45 days');

    -- admin_id (PeoplePortal Admin)
    INSERT INTO hr_requests (id, employee_id, type, status, reason, created_at_utc) VALUES
    (gen_random_uuid(), admin_id, 'DataUpdate', 'Approved', 'Actualización de cuenta bancaria para depósitos.', NOW() - interval '100 days');

END $$;
