import React from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { UserOutlined, MailOutlined, IdcardOutlined, PhoneOutlined } from '@ant-design/icons';
import { useAuth } from '../../services/AuthContext';

const FormCadastroGranja = ({ onSubmit, erros, agroLista }) => {
    const { userData } = useAuth();

    const [formData, setFormData] = React.useState({
        nomePropriedade: '',
        agroindustriaId: userData.role !== 'ADMIN' ? userData.agroindustriaId : '',
        cnpj: '',
        email: '',
        telefone: ''
    });

    const handleChange = (e) => {
        let { name, value } = e.target;

        // Remove máscara de campos específicos
        if (['cnpj', 'telefone'].includes(name)) {
            value = value.replace(/\D/g, '');
        }

        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <form className="w-full" onSubmit={handleSubmit}>
            {/* Nome da Granja */}
            <InputSemBordaComLabel
                label="Nome da Granja"
                name="nomePropriedade"
                value={formData.nomePropriedade}
                onChange={handleChange}
                placeholder="Nome da granja"
                icone={<UserOutlined className="text-green-dark" />}
                erro={erros.nomePropriedade}
            />

            {/* Agroindústria */}
            {userData.role === 'ADMIN' && (
                <>
                    <select
                        name="agroindustriaId"
                        value={formData.agroindustriaId}
                        onChange={handleChange}
                        className="w-full border-b border-gray-300 py-2 px-3 mb-4 focus:outline-none focus:border-green-dark"
                    >
                        <option value="">Selecione a agroindústria</option>
                        {agroLista?.map((agro) => (
                            <option key={agro.id} value={agro.id}>
                                {agro.nomeFantasia}
                            </option>
                        ))}
                    </select>
                    {erros.agroindustriaId && <p className="text-red-500 text-sm">{erros.agroindustriaId}</p>}
                </>
            )}

            {/* CNPJ */}
            <InputSemBordaComLabel
                label="CNPJ"
                name="cnpj"
                value={formData.cnpj}
                onChange={handleChange}
                placeholder="XX.XXX.XXX/XXXX-XX"
                icone={<IdcardOutlined className="text-green-dark" />}
                erro={erros.cnpj}
                mask="99.999.999/9999-99"
            />

            {/* Email */}
            <InputSemBordaComLabel
                label="E-mail"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="exemplo@email.com"
                icone={<MailOutlined className="text-green-dark" />}
                erro={erros.email}
            />

            {/* Telefone */}
            <InputSemBordaComLabel
                label="Telefone"
                name="telefone"
                type="tel"
                value={formData.telefone}
                onChange={handleChange}
                placeholder="(XX) XXXXX-XXXX"
                icone={<PhoneOutlined className="text-green-dark" />}
                erro={erros.telefone}
                mask="(99) 99999-9999"
            />

            <div className="w-1/2 mx-auto mt-6">
                <BotaoPrimario texto="Cadastrar Granja" type="submit" />
            </div>
        </form>
    );
};

export default FormCadastroGranja;
