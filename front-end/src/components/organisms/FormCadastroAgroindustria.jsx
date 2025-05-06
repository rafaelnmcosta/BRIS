import React from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { UserOutlined, MailOutlined, IdcardOutlined, PhoneOutlined } from '@ant-design/icons';
import { useValidation } from '../../services/ValidationContext';

const FormCadastroAgroindustria = ({
    onSubmit,
    erros,
    vinculos,
    onAbrirModal
}) => {

    const [formData, setFormData] = React.useState({
        nomeFantasia: '',
        razaoSocial: '',
        cnpj: ''
        //telefone: '',
        //email: '',
    });

    const handleChange = (e) => {
        let { name, value } = e.target;
      
        // remove a máscara de campos específicos
        if (['cnpj'].includes(name)) {
          value = value.replace(/\D/g, '');
        }
      
        setFormData({ ...formData, [name]: value });
      };

    const handleSubmit = (e) => {
        e.preventDefault();

        onSubmit({
            ...formData,
            vinculos
        });
    };

    return (
        <>
            <form className="w-full" onSubmit={handleSubmit}>
                {/* Campos do formulário */}
                <InputSemBordaComLabel
                    label="Nome fantasia"
                    name="nomeFantasia"
                    value={formData.nomeFantasia}
                    onChange={handleChange}
                    placeholder="Nome fantasia da agroindústria"
                    icone={<UserOutlined className="text-green-dark" />}
                    erro={erros.nomeFantasia}
                />

                <InputSemBordaComLabel
                    label="Razão social"
                    name="razaoSocial"
                    value={formData.razaoSocial}
                    onChange={handleChange}
                    placeholder="Razão social da agroindústria"
                    icone={<MailOutlined className="text-green-dark" />}
                    erro={erros.razaoSocial}
                />

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

                {/* Adicionar email e telefone na agroindustria depois talvezzzz???

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
                */}
                <div className='w-1/2 mx-auto'>
                    <BotaoPrimario texto="Cadastrar agroindústria" type="submit" />
                </div>
            </form>
        </>
    );
};

export default FormCadastroAgroindustria;