import React, { useEffect, useState } from 'react';
import TemplateLista from '../components/templates/TemplateLista';

const Lista = () => {
    const [usuarios, setUsuarios] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchusuarios = async () => {
            try {
                console.log("Fazendo fetch dos usuários")
                const response = await usuarios.listar();
                setUsuarios(response);
            } catch (err) {
                setError('Erro ao carregar os usuários.');
            } finally {
                setLoading(false);
            }
        };

        fetchusuarios();
    });

    if (loading) {
        return <p>Carregando usuários...</p>;
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return <TemplateLista tipo={"Usuário"} usuarios={usuarios} />;
};

export default Lista;