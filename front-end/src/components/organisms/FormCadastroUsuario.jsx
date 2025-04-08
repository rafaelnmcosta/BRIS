import React from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { UserOutlined, MailOutlined, IdcardOutlined, PhoneOutlined, LockOutlined, PlusOutlined } from '@ant-design/icons';

const FormCadastroUsuario = ({ onSubmit, erros, vinculos, onAbrirModal }) => {
    const [formData, setFormData] = React.useState({
        nome: '',
        email: '',
        cpf: '',
        telefone: '',
        senha: '',
        confirmarSenha: ''
    });

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        
        if(vinculos.length === 0) {
            erros = (prev => ({...prev, vinculos: 'Adicione pelo menos um vínculo'}));
            return;
        }
        
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
                    label="Nome"
                    name="nome"
                    value={formData.nome}
                    onChange={handleChange}
                    placeholder="Nome completo"
                    icone={<UserOutlined className="text-green-dark" />}
                    erro={erros.nome}
                />

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
                    label="CPF"
                    name="cpf"
                    value={formData.cpf}
                    onChange={handleChange}
                    placeholder="XXX.XXX.XXX-XX"
                    icone={<IdcardOutlined className="text-green-dark" />}
                    erro={erros.cpf}
                />

                <InputSemBordaComLabel
                    label="Telefone"
                    name="telefone"
                    type="tel"
                    value={formData.telefone}
                    onChange={handleChange}
                    placeholder="(XX) XXXXX-XXXX"
                    icone={<PhoneOutlined className="text-green-dark" />}
                />

                <InputSemBordaComLabel
                    label="Senha"
                    name="senha"
                    type="password"
                    value={formData.senha}
                    onChange={handleChange}
                    placeholder="******"
                    icone={<LockOutlined className="text-green-dark" />}
                    erro={erros.senha}
                />

                <InputSemBordaComLabel
                    label="Confirme a senha"
                    name="confirmarSenha"
                    type="password"
                    value={formData.confirmarSenha}
                    onChange={handleChange}
                    placeholder="******"
                    icone={<LockOutlined className="text-green-dark" />}
                    erro={erros.confirmarSenha}
                />

                {/* Seção de Vínculos */}
                <div className="my-6">
                    <div className="flex justify-between items-center mb-4">
                        <h3 className="text-lg font-semibold">Vínculos</h3>
                        <button
                            type="button"
                            onClick={onAbrirModal}
                            className="flex items-center gap-2 text-green-dark hover:text-green"
                        >
                            <PlusOutlined /> Novo Vínculo
                        </button>
                    </div>

                    {/* Listagem de vínculos */}
                    {vinculos.map((vinculo, index) => (
                        <div key={index} className="p-3 mb-2 border rounded-lg">
                            <p>Perfil: {vinculo.roleId}</p>
                            {vinculo.granjaId && <p>Granja: {vinculo.granjaId}</p>}
                            {vinculo.agroindustriaId && <p>Agroindústria: {vinculo.agroindustriaId}</p>}
                        </div>
                    ))}

                    {erros.vinculos && <p className="text-red-500 text-sm">{erros.vinculos}</p>}
                </div>
                <div className='w-1/2 mx-auto'>
                    <BotaoPrimario texto="Cadastrar Usuário" type="submit" />
                </div>
            </form>
        </>
    );
};

export default FormCadastroUsuario;