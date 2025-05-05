import React, { useState, useEffect } from 'react';
import { Form } from 'antd';
import { useAuth } from '../../services/AuthContext';
import SeletorBuscaDinamica from '../molecules/SeletorBuscaDinamica';
import BotaoPrimario from '../atoms/BotaoPrimario';

const ModalVinculos = ({ visible, onCancelar, onSalvar }) => {
    const [form] = Form.useForm();
    const { userData } = useAuth();
    const [selectedRole, setSelectedRole] = useState(null);
    const [filteredRoles, setFilteredRoles] = useState([]);
    const agroindustriaSelecionada = Form.useWatch('agroindustriaId', form);
    console.log('🟡 agroindustriaSelecionada:', agroindustriaSelecionada);
    console.log(userData.role)

    useEffect(() => {
        const filterRoles = () => {
            let roles = [
                { value: 1, label: 'Administrador' },
                { value: 2, label: 'Gestor de Granja' },
                { value: 3, label: 'Gestor de Agroindústria' },
                { value: 4, label: 'Técnico' },
                { value: 5, label: 'Visualizador' }
            ];

            if (userData.role === 'GESTOR_AGRO') {
                roles = roles.filter(r => r.value !== 1);
            }
            else if (userData.role === 'GESTOR_GRANJA') {
                roles = roles.filter(r => [2, 4, 5].includes(r.value));
            }

            setFilteredRoles(roles);
        };
        filterRoles();
    }, [userData.role]);

    useEffect(() => {
        console.log('🟢 agroindustriaSelecionada mudou:', agroindustriaSelecionada);
    }, [agroindustriaSelecionada]);

    const handleCancel = () => {
        form.resetFields();
        setSelectedRole(null);
        onCancelar();
    };

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            let payload = { roleId: values.roleId };

            console.log(values)

            if (userData.role === 'ADMIN') {
                payload.agroindustriaId = values.agroindustriaId;
                payload.granjaId = values.granjaId;
            }
            else if (userData.role === 'GESTOR_AGRO') {
                payload.agroindustriaId = userData.agroindustriaId;
                payload.granjaId = values.granjaId;
            }
            else {
                payload.agroindustriaId = values.agroindustriaId;
                payload.granjaId = values.granjaId;
            }

            onSalvar(payload);
            handleCancel();
        } catch (error) {
            console.error('Erro no formulário:', error);
        }
    };

    useEffect(() => {
        if (visible) {
            let initialValues = {};

            if (userData.role === 'GESTOR_AGRO') {
                initialValues = {
                    agroindustriaId: userData.agroindustriaId
                };
            }
            else if (userData.role === 'GESTOR_GRANJA') {
                initialValues = {
                    agroindustriaId: userData.agroindustriaId,
                    granjaId: userData.granjaId
                };
            }
            form.setFieldsValue(initialValues);
        }
    }, [visible, userData, form]);

    return (
        visible && (
            <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
                <div className="bg-white p-6 rounded-lg w-96 shadow-xl">
                    <div className="flex justify-between items-center mb-4 border-b pb-2">
                        <h2 className="text-xl font-bold text-green-dark">Adicionar Vínculo</h2>
                        <button onClick={handleCancel} className="text-gray-500 hover:text-gray-700 text-2xl">
                            ×
                        </button>
                    </div>

                    <Form form={form} layout="vertical">
                        <Form.Item
                            name="roleId"
                            label="Perfil"
                            rules={[{ required: true, message: 'Selecione o perfil' }]}
                        >
                            <select
                                className="w-full p-2 border-b-2 border-green-dark focus:outline-none"
                                onChange={(e) => setSelectedRole(Number(e.target.value))}
                            >
                                <option value="">Selecione o perfil</option>
                                {filteredRoles.map(option => (
                                    <option key={option.value} value={option.value}>
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                        </Form.Item>

                        {/* Campo Agroindústria */}
                        {selectedRole && [2, 3, 4, 5].includes(selectedRole) && (
                            <Form.Item
                                name="agroindustriaId"
                                label="Agroindústria"
                                rules={[{
                                    required: [2, 3, 4, 5].includes(selectedRole),
                                    message: 'Selecione a agroindústria'
                                }]}
                            >
                                <SeletorBuscaDinamica
                                    endpoint="/agroindustrias"
                                    placeholder={userData.role === 'ADMIN' ? "Busque por razão social ou CNPJ" : userData.agroindustria}
                                    disabled={userData.role !== 'ADMIN'}
                                    onChange={(value) => {
                                        form.setFieldValue('agroindustriaId', value);
                                        form.setFieldValue('granjaId', null); // Limpa a granja ao alterar agroindustria
                                    }}
                                />
                            </Form.Item>
                        )}

                        {/* Campo Granja */}
                        {selectedRole && [2, 4, 5].includes(selectedRole) && agroindustriaSelecionada && (
                            <Form.Item
                                name="granjaId"
                                label="Granja"
                                rules={[{
                                    required: [2, 4, 5].includes(selectedRole),
                                    message: 'Selecione a granja'
                                }]}
                            >
                                <SeletorBuscaDinamica
                                    endpoint="/granjas"
                                    placeholder={userData.role === 'GESTOR_GRANJA' ? userData.granja : "Busque por nome fantasia ou CNPJ"}
                                    disabled={userData.role === 'GESTOR_GRANJA'}
                                    agroindustriaId={agroindustriaSelecionada}
                                    onChange={(value) => {
                                        form.setFieldValue('granjaId', value);
                                    }}
                                />
                            </Form.Item>
                        )}


                        <div className="flex justify-end gap-3 mt-6">
                            <button
                                onClick={handleCancel}
                                className="px-4 py-2 text-gray-600 hover:text-gray-800 transition-colors"
                            >
                                Cancelar
                            </button>
                            <BotaoPrimario
                                texto="Salvar"
                                onClick={handleOk}
                                className="px-4 py-2"
                            />
                        </div>
                    </Form>
                </div>
            </div>
        )
    );
};

export default ModalVinculos;