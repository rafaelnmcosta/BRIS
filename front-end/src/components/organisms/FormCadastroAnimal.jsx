import React, { useEffect, useState } from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { UserOutlined, IdcardOutlined, NumberOutlined, SafetyCertificateOutlined } from '@ant-design/icons';
import { useAuth } from '../../services/AuthContext';

const FormCadastroAnimal = ({
    onSubmit,
    erros,
    carregarAgroindustrias,
    carregarGranjas,
    carregarUsuarios
}) => {
    const { userData } = useAuth();

    const [formData, setFormData] = useState({
        linhagem: '',
        idade: '',
        peso: '',
        status: null,
        granjaId: userData.granjaId || '',
        usuarioResponsavelId: userData.id,
        ativo: true,
        agroindustriaId: userData.role !== 'ADMIN' ? userData.agroindustriaId : ''
    });

    const [agroLista, setAgroLista] = useState([]);
    const [granjaLista, setGranjaLista] = useState([]);
    const [usuarioLista, setUsuarioLista] = useState([]);

    // Carrega agroindústrias no mount
    useEffect(() => {
        if (carregarAgroindustrias && userData.role === 'ADMIN') {
            carregarAgroindustrias().then(setAgroLista);
        }
    }, []);

    // Carrega granjas sempre que a agroindustria muda
    useEffect(() => {
        if (formData.agroindustriaId && carregarGranjas) {
            carregarGranjas(formData.agroindustriaId).then(setGranjaLista);
        }
    }, [formData.agroindustriaId]);

    // Carrega usuários para dropdown quando necessário
    useEffect(() => {
        if ((userData.role === 'GESTOR_AGRO' || userData.role === 'GESTOR_GRANJA') && carregarUsuarios) {
            carregarUsuarios().then(setUsuarioLista);
        }
    }, []);

    const handleChange = (e) => {
        let { name, value } = e.target;
        // Remove máscara se necessário
        if (name === 'peso') value = parseFloat(value) || '';
        if (name === 'idade') value = parseInt(value) || '';
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <form className="w-full" onSubmit={handleSubmit}>
            {/* Linhagem */}
            <InputSemBordaComLabel
                label="Linhagem"
                name="linhagem"
                value={formData.linhagem}
                onChange={handleChange}
                placeholder="Linhagem do animal"
                icone={<UserOutlined className="text-green-dark" />}
                erro={erros.linhagem}
            />

            {/* Idade */}
            <InputSemBordaComLabel
                label="Idade"
                name="idade"
                type="number"
                value={formData.idade}
                onChange={handleChange}
                placeholder="Idade do animal"
                icone={<NumberOutlined className="text-green-dark" />}
                erro={erros.idade}
            />

            {/* Peso */}
            <InputSemBordaComLabel
                label="Peso"
                name="peso"
                type="number"
                value={formData.peso}
                onChange={handleChange}
                placeholder="Peso do animal (kg)"
                icone={<SafetyCertificateOutlined className="text-green-dark" />}
                erro={erros.peso}
            />

            {/* Status (só exibe para info, não altera) */}
            {formData.status !== null && (
                <InputSemBordaComLabel
                    label="Status"
                    name="status"
                    value={formData.status === true ? 'Aprovado' : formData.status === false ? 'Reprovado' : 'Aguardando avaliação'}
                    disabled
                    icone={<SafetyCertificateOutlined className="text-green-dark" />}
                />
            )}

            {/* Agroindústria (apenas admin) */}
            {userData.role === 'ADMIN' && (
                <>
                    <select
                        name="agroindustriaId"
                        value={formData.agroindustriaId}
                        onChange={handleChange}
                        className="w-full border-b border-gray-300 py-2 px-3 mb-4 focus:outline-none focus:border-green-dark"
                    >
                        <option value="">Selecione a agroindústria</option>
                        {agroLista?.map(a => (
                            <option key={a.id} value={a.id}>{a.nomeFantasia}</option>
                        ))}
                    </select>
                    {erros.agroindustriaId && <p className="text-red-500 text-sm">{erros.agroindustriaId}</p>}
                </>
            )}

            {/* Granja */}
            {(userData.role === 'ADMIN' || userData.role === 'GESTOR_AGRO') && (
                <>
                    <select
                        name="granjaId"
                        value={formData.granjaId}
                        onChange={handleChange}
                        className="w-full border-b border-gray-300 py-2 px-3 mb-4 focus:outline-none focus:border-green-dark"
                    >
                        <option value="">Selecione a granja</option>
                        {granjaLista?.map(g => (
                            <option key={g.id} value={g.id}>{g.nomePropriedade}</option>
                        ))}
                    </select>
                    {erros.granjaId && <p className="text-red-500 text-sm">{erros.granjaId}</p>}
                </>
            )}

            {/* Usuário Responsável */}
            {(userData.role === 'ADMIN' || userData.role === 'GESTOR_AGRO' || userData.role === 'GESTOR_GRANJA') && (
                <>
                    {userData.role === 'ADMIN' ? (
                        <input
                            name="usuarioResponsavelId"
                            value={formData.usuarioResponsavelId}
                            onChange={handleChange}
                            placeholder="ID do usuário responsável"
                            className="w-full border-b border-gray-300 py-2 px-3 mb-4 focus:outline-none focus:border-green-dark"
                        />
                    ) : (
                        <select
                            name="usuarioResponsavelId"
                            value={formData.usuarioResponsavelId}
                            onChange={handleChange}
                            className="w-full border-b border-gray-300 py-2 px-3 mb-4 focus:outline-none focus:border-green-dark"
                        >
                            <option value="">Selecione o usuário responsável</option>
                            {usuarioLista?.map(u => (
                                <option key={u.id} value={u.id}>{u.nome}</option>
                            ))}
                        </select>
                    )}
                    {erros.usuarioResponsavelId && <p className="text-red-500 text-sm">{erros.usuarioResponsavelId}</p>}
                </>
            )}

            {/* Ativo */}
            <div className="mb-4 flex items-center gap-2">
                <input
                    type="checkbox"
                    name="ativo"
                    checked={formData.ativo}
                    onChange={e => setFormData({ ...formData, ativo: e.target.checked })}
                />
                <label>Ativo</label>
            </div>

            <div className="w-1/2 mx-auto mt-6">
                <BotaoPrimario texto="Cadastrar Animal" type="submit" />
            </div>
        </form>
    );
};

export default FormCadastroAnimal;
